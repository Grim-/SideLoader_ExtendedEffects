using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    public abstract class BasePortal : MonoBehaviour
    {
        protected string ownerUID;
        protected bool isFirstPortal;
        protected float teleportCooldown = 0.5f;
        protected float cooldownTimer = 0f;
        protected bool canTeleport = true;

        protected float lifeTime = -1f;
        protected float currentTime;

        protected ParticleSystem portalParticles;
        protected ParticleSystemRenderer particleRenderer;

        protected const string PORTAL_RENDER_PATH = "Visual/RenderPortal";

        public virtual void Initialize(string characterUID, bool isFirst, float teleportCooldown, bool canTeleportAI, bool canTeleportPlayers, bool canTeleportProjectiles, float lifetime = -1f)
        {
            if (string.IsNullOrEmpty(characterUID))
            {
                Debug.LogError("Portal Initialize: Invalid character UID");
                return;
            }

            ownerUID = characterUID;
            isFirstPortal = isFirst;
            lifeTime = lifetime;
            currentTime = 0f;
            this.teleportCooldown = teleportCooldown;



            if (!SetupPortalComponents())
            {
                Debug.LogError("Portal Initialize: Failed to setup portal components");
                return;
            }
        }

        protected virtual bool SetupPortalComponents()
        {
            Transform renderPortalTransform = transform.Find(PORTAL_RENDER_PATH);
            if (renderPortalTransform == null)
            {
                Debug.LogError($"SetupPortalComponents: Cannot find {PORTAL_RENDER_PATH}");
                return false;
            }

            portalParticles = renderPortalTransform.GetComponent<ParticleSystem>();
            if (portalParticles == null)
            {
                Debug.LogError("SetupPortalComponents: ParticleSystem not found on RenderPortal");
                return false;
            }

            particleRenderer = portalParticles.GetComponent<ParticleSystemRenderer>();
            if (particleRenderer == null)
            {
                Debug.LogError("SetupPortalComponents: ParticleSystemRenderer not found");
                return false;
            }

            return true;
        }

        protected virtual void Update()
        {
            UpdateLifetime();
            UpdateTeleportCooldown();
        }
        public void SetLifetime(float life)
        {
            if (life < 0)
            {
                Debug.LogWarning("SetLifetime: Negative lifetime specified");
            }
            this.lifeTime = life;
        }

        protected void UpdateLifetime()
        {
            if (lifeTime <= 0) return;

            currentTime += Time.deltaTime;
            if (currentTime >= lifeTime)
            {
                Cleanup();
            }
        }

        protected void UpdateTeleportCooldown()
        {
            if (canTeleport) return;

            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                EnableParticleSystems();
                canTeleport = true;
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (!canTeleport) return;

            OnTriggeredTeleporter(other);
        }

        protected abstract void OnTriggeredTeleporter(Collider other);

        public virtual void DisableTeleport()
        {
            canTeleport = false;
            cooldownTimer = teleportCooldown;
            DisableParticleSystems();
        }

        public virtual void DisableParticleSystems()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                if (ps.gameObject.name.Contains("Circle")) continue;
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }

        public virtual void EnableParticleSystems()
        {
            var particleSystems = GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                if (!ps.isPlaying) ps.Play();
            }
        }

        public virtual void Cleanup()
        {
            Destroy(gameObject);
        }
    }
}

