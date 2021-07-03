using System.Collections.Generic;
using System.Linq;
using Divine;
using Divine.SDK.Prediction;
using Divine.SDK.Prediction.Collision;
using O9K.AIO.Heroes.Base;
using O9K.AIO.Modes.Permanent;
using O9K.Core.Entities.Abilities.Base.Components.Base;
using O9K.Core.Entities.Abilities.Heroes.Invoker;
using O9K.Core.Entities.Units;
using O9K.Core.Helpers;
using SharpDX;

namespace O9K.AIO.Heroes.Invoker.Modes
{
    internal class AutoSunStrikeMode : PermanentMode
    {
        private readonly AutoSunStrikeModeMenu modeMenu;
        private readonly Sleeper sleeper = new Sleeper();
        private readonly MultiSleeper<string> runningSleeper = new MultiSleeper<string>();

        public AutoSunStrikeMode(BaseHero baseHero, AutoSunStrikeModeMenu menu)
            : base(baseHero, menu)
        {
            modeMenu = menu;
        }

        private readonly string effectKey = "AutoSS";
        private readonly Dictionary<uint, float> _timeDictionary = new Dictionary<uint, float>();

        private void flush()
        {
            ParticleManager.RemoveParticle(effectKey);
        }

        public override void Dispose()
        {
            flush();
            base.Dispose();
        }

        public override void Disable()
        {
            flush();
            base.Disable();
        }

        private void FlushTiming(Unit9 target)
        {
            var handle = target.Handle;
            if (_timeDictionary.ContainsKey(handle))
                _timeDictionary.Remove(handle);
        }

        private bool CheckForTiming(Unit9 target, out float timing)
        {
            var handle = target.Handle;
            var currentTime = GameManager.RawGameTime;
            if (_timeDictionary.TryGetValue(handle, out var time))
            {
                timing = currentTime - time;
                return timing >= modeMenu.delay / 1000f;
            }

            timing = 0f;
            _timeDictionary.Add(handle, currentTime);
            return false;
        }

        private bool CheckForTiming(Unit9 target)
        {
            return CheckForTiming(target, out _);
        }

        private bool ShouldCast(Unit9 target, SunStrike sunStrike, ref Vector3 position)
        {
            if (modeMenu.killStealOnly)
            {
                var willDie = sunStrike.GetDamage(target) > target.Health + target.HealthRegeneration * 2;
                if (!willDie)
                {
                    FlushTiming(target);
                    return false;
                }

                var input = new PredictionInput
                {
                    Owner = Owner.Hero,
                    AreaOfEffect = false,
                    CollisionTypes = CollisionTypes.None,
                    Delay = sunStrike.CastPoint + sunStrike.ActivationDelay,
                    Speed = float.MaxValue,
                    Range = float.MaxValue,
                    Radius = sunStrike.Radius,
                    PredictionSkillshotType = PredictionSkillshotType.SkillshotCircle,
                };

                input = input.WithTarget(target);

                var hookOutput = PredictionManager.GetPrediction(input);
                
                var cantHitWithPrediction = hookOutput.HitChance is not (HitChance.Medium or HitChance.High or HitChance.VeryHigh) || !target.IsMoving || target.IsRotating;

                if (cantHitWithPrediction && !ChainStun(target, sunStrike))
                {
                    FlushTiming(target);
                    return false;
                }

                if (!CheckForTiming(target))
                {
                    return false;
                }

                position = hookOutput.CastPosition;
            }
            else
            {
                if (!ChainStun(target, sunStrike))
                {
                    FlushTiming(target);
                    return false;
                }

                position = target.Position;
            }

            return true;
        }

        private bool ChainStun(Unit9 target, IActiveAbility sunStrikeAbility)
        {
            var immobile = target.GetImmobilityDuration();
            if (immobile <= 0)
            {
                immobile = target.GetInvulnerabilityDuration();
                if (immobile <= 0)
                {
                    return false;
                }
            }


            var hitTime = sunStrikeAbility.GetHitTime(target) /*+ .1f*/;

            if (target.IsInvulnerable)
            {
                hitTime -= 0.1f;
            }

            return hitTime > immobile;
        }

        private void CameraAction(Vector3 position)
        {
            if (!modeMenu.moveCamera) return;
            var screenPosition = RendererManager.WorldToScreen(position, true);
            if (!screenPosition.IsZero) return;
            var consolePosition = $"{position.X} {position.Y}";
            GameManager.ExecuteCommand($"dota_camera_set_lookatpos {consolePosition}");
        }

        protected override void Execute()
        {
            if (sleeper.IsSleeping)
                return;
            if (Owner.Hero.Abilities.FirstOrDefault(x =>
                    x.Id == AbilityId.invoker_sun_strike) is SunStrike sunStrike && sunStrike.CanBeCasted()
                                                                                 && (sunStrike.CanBeInvoked || sunStrike.IsInvoked))
            {
                var targetPosition = Vector3.Zero;
                
                  /*TargetManager.AllEnemyHeroes.ForEach(target =>
                  {
                      var input = new PredictionInput
                      {
                          Owner = Owner.Hero,
                          AreaOfEffect = false,
                          CollisionTypes = CollisionTypes.Trees | CollisionTypes.AllyCreeps | CollisionTypes.EnemyCreeps,
                          Delay = sunStrike.CastPoint + sunStrike.ActivationDelay,
                          Speed = float.MaxValue,
                          Range = float.MaxValue,
                          Radius = sunStrike.Radius,
                          PredictionSkillshotType = PredictionSkillshotType.SkillshotCircle,
                      };
                  
                      input = input.WithTarget(target);
                  
                      var hookOutput = PredictionManager.GetPrediction(input);
                  
                      if (hookOutput.HitChance == HitChance.Collision)
                      {
                          ParticleManager.LineParticle("key", target.Position, hookOutput.CastPosition, 50, Color.Green);
                      }
                      else
                      {
                          ParticleManager.LineParticle("key", target.Position, hookOutput.CastPosition, 50, Color.Red);
                      }
                      
                      
                  });*/
                
                var enemies = TargetManager.AllEnemyHeroes.Where(x => ShouldCast(x, sunStrike, ref targetPosition))
                    .OrderBy(x => x.HealthPercentage)
                    .ToList();

                var target = enemies.FirstOrDefault();
                if (target != null)
                {
                    CameraAction(targetPosition);
                    sunStrike.UseAbility(targetPosition);
                }
                else
                {
                    flush();
                }

                sleeper.Sleep(0.01f);
            }
        }
    }
}