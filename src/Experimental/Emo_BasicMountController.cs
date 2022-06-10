namespace SideLoader_ExtendedEffects.Released
{
    using SideLoader;
    using UnityEngine;

    namespace SideLoader_ExtendedEffects
    {
        public class Emo_BasicMountController : MonoBehaviour
        {
            private Character MountTarget;
            private float MoveSpeed = 5f;
            private Animator Animator;
            private Vector3 OriginalCameraOffset;
            public CharacterController Controller;

            public void Awake()
            {
                Animator = GetComponent<Animator>();
                Controller = GetComponent<CharacterController>();
            }

            public void SetMountTarget(Character mountTarget)
            {
                MountTarget = mountTarget;
                DisableCharacterComponents(MountTarget);
                TryParentToMountPoint(MountTarget, gameObject);
                OriginalCameraOffset = MountTarget.CharacterCamera.Offset;
                UpdateCharacterCamera(MountTarget, OriginalCameraOffset + new Vector3(0, 0, -4));
            }

            public void Update()
            {
                if (MountTarget == null)
                {
                    return;
                }

                //FaceTransformTowardsCamera();
                UpdateMoveInput();
                UpdateAnimator();
            }
            private void EnableCharacterComponents(Character _affectedCharacter)
            {
                //_affectedCharacter.enabled = true;
                _affectedCharacter.CharMoveBlockCollider.enabled = true;
                _affectedCharacter.CharacterController.enabled = true;
                _affectedCharacter.CharacterControl.enabled = true;
                _affectedCharacter.Animator.enabled = true;
                _affectedCharacter.Animator.Update(Time.deltaTime);
            }

            private void DisableCharacterComponents(Character _affectedCharacter)
            {
                _affectedCharacter.CharMoveBlockCollider.enabled = false;
                _affectedCharacter.CharacterController.enabled = false;
                _affectedCharacter.CharacterControl.enabled = false;
                _affectedCharacter.SpellCastAnim(Character.SpellCastType.Sit, Character.SpellCastModifier.Immobilized, 1);
                //_affectedCharacter.enabled = false;
                //_affectedCharacter.Animator.StopPlayback();
                //_affectedCharacter.Animator.enabled = false;
            }

            private void TryParentToMountPoint(Character _affectedCharacter, GameObject MountInstance)
            {
                Transform mountPointTransform = TryGetMountPoint();
                if (mountPointTransform != null)
                {
                    _affectedCharacter.transform.parent = mountPointTransform;
                    _affectedCharacter.transform.localPosition = Vector3.zero;
                    _affectedCharacter.transform.localEulerAngles = Vector3.zero;
                }
                else
                {
                    _affectedCharacter.transform.parent = MountInstance.transform;
                    _affectedCharacter.transform.localPosition = Vector3.zero;
                    _affectedCharacter.transform.localEulerAngles = Vector3.zero;

                }
            }
            private void UpdateCharacterCamera(Character _affectedCharacter, Vector3 NewOffset)
            {
                _affectedCharacter.CharacterCamera.Offset = NewOffset;
            }
            private Transform TryGetMountPoint()
            {
                return transform.FindInAllChildren("SL_MOUNTPOINT");
            }

            private void FaceTransformTowardsCamera()
            {
                Vector3 YOnly = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
                transform.forward = YOnly;
            }

            private void UpdateAnimator()
            {
                Animator.SetFloat("Move X", Controller.velocity.x);
                Animator.SetFloat("Move Y", Controller.velocity.z);
            }

            private void UpdateMoveInput()
            {
                if (CustomKeybindings.GetKeyDown("TEST_DISMOUNT_BUTTON") && MountTarget != null)
                {
                    DoDismount();
                }



                Vector3 BaseInput = new Vector3(ControlsInput.MoveHorizontal(MountTarget.OwnerPlayerSys.PlayerID), 0, ControlsInput.MoveVertical(MountTarget.OwnerPlayerSys.PlayerID));
                Vector3 Input = Camera.main.transform.TransformDirection(BaseInput);

                Vector3 CameraRelativeInputNoY = new Vector3(Input.x, 0, Input.z);
                transform.forward = Vector3.RotateTowards(transform.forward, transform.forward + CameraRelativeInputNoY, 60f * Time.deltaTime, 10f);
                Controller.SimpleMove(Input.normalized * MoveSpeed);
            }

            private void DoDismount()
            {
                EnableCharacterComponents(MountTarget);
                UpdateCharacterCamera(MountTarget, OriginalCameraOffset);
                MountTarget.StatusEffectMngr.RemoveStatusWithIdentifierName("MountedStatusEffect");
                MountTarget.transform.parent = null;
                MountTarget.transform.position = transform.position;
                MountTarget.transform.eulerAngles = Vector3.zero;
                MountTarget = null;

                Destroy(gameObject);
            }
        }
    }

}
