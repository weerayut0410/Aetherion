using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool attack;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true; // ��������������ö�� true ���� false ���� �������Ѻ�����ҡ�����������仵�����á���
        public bool cursorInputForLook = true; // ����������ͧ�������

        // ��������� public ������� ThirdPersonController ����ö��Ҷ֧ʶҹй����
        public bool isUIMode = false; // ʶҹз��͡��������������� UI (������ʴ�, ����Ф���ش)

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            // ��Ǩ�ͺ����������������� UI ��͹�����Ѻ Input �������͹���
            if (!isUIMode)
            {
                MoveInput(value.Get<Vector2>());
            }
            else
            {
                MoveInput(Vector2.zero); // �ҡ��������� UI �����ش�������͹���
            }
        }

        public void OnLook(InputValue value)
        {
            // ��Ǩ�ͺ����������������� UI ��� cursorInputForLook �繨�ԧ ��͹�����Ѻ Input ����ͧ
            if (cursorInputForLook && !isUIMode)
            {
                LookInput(value.Get<Vector2>());
            }
            else
            {
                LookInput(Vector2.zero); // �ҡ��������� UI �����ش����ͧ
            }
        }

        public void OnJump(InputValue value)
        {
            if (!isUIMode)
            {
                JumpInput(value.isPressed);
            }
            else
            {
                JumpInput(false); // �ҡ��������� UI �����ش��á��ⴴ
            }
        }

        public void OnSprint(InputValue value)
        {
            if (!isUIMode)
            {
                SprintInput(value.isPressed);
            }
            else
            {
                SprintInput(false); // �ҡ��������� UI �����ش Sprint
            }
        }

        public void OnAttack(InputValue value)
        {
            if (!isUIMode)
            {
                AttackInput(value.isPressed);
            }
            else
            {
                AttackInput(false); // �ҡ��������� UI �����ش����
            }
        }

        // �����ѧ��ѹ����Ѻ�Ѻ Input �ҡ���� Esc
        public void OnEscape(InputValue value) // ��ͧ���ҧ Action "Escape" � Input Actions Asset ����
        {
            if (value.isPressed)
            {
                ToggleUIMode();
            }
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }
        public void AttackInput(bool newAttackState)
        {
            attack = newAttackState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // ��ҨФǺ��� Cursor ��ҹ ToggleUIMode ᷹
            // SetCursorState(cursorLocked); // ������������ź�͡
            if (hasFocus && !isUIMode) // ����ͻ���पѹ���Ѻ focus ����������������� UI �����͡ Cursor
            {
                SetCursorState(true);
            }
            else if (!hasFocus && !isUIMode) // ������� focus ����������������� UI ���Ŵ��͡
            {
                SetCursorState(false);
            }
            // ������������ UI �������� (isUIMode �� true) ����ͧ������ ���� Cursor �١�Ѵ����� ToggleUIMode ����
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !newState; // ����� Cursor �ʴ�����������͡ ��Ы�͹�������͡
        }

        // �ѧ��ѹ����Ѻ��Ѻ���� UI (������ʴ�/����, ����Ф���ش/����͹���)
        public void ToggleUIMode()
        {
            isUIMode = !isUIMode; // ��Ѻʶҹ����� UI
            SetCursorState(!isUIMode); // ��������� UI (true) ���Ŵ��͡ Cursor (false), �������� (false) �����͡ Cursor (true)

            // ���� Input �ͧ����Ф÷ѹ�շ����Ѻ���� ���ͻ�ͧ�ѹ Input ��ҧ
            move = Vector2.zero;
            look = Vector2.zero;
            jump = false;
            sprint = false;
            attack = false;

            // **Optional:** ����� UI Panel ����Ѻ Pause Menu ����Դ/�Դ�ç���
            // if (pauseMenuPanel != null)
            // {
            //     pauseMenuPanel.SetActive(isUIMode);
            // }
        }
    }
}