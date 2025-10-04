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
        public bool cursorLocked = true; // ค่าเริ่มต้นสามารถเป็น true หรือ false ก็ได้ ขึ้นอยู่กับว่าอยากให้เมาส์หายไปตั้งแต่แรกไหม
        public bool cursorInputForLook = true; // จะใช้เมาส์มองหรือไม่

        // เพิ่มตัวแปร public เพื่อให้ ThirdPersonController สามารถเข้าถึงสถานะนี้ได้
        public bool isUIMode = false; // สถานะที่บอกว่าเราอยู่ในโหมด UI (เมาส์แสดง, ตัวละครหยุด)

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            // ตรวจสอบว่าไม่ได้อยู่ในโหมด UI ก่อนที่จะรับ Input การเคลื่อนไหว
            if (!isUIMode)
            {
                MoveInput(value.Get<Vector2>());
            }
            else
            {
                MoveInput(Vector2.zero); // หากอยู่ในโหมด UI ให้หยุดการเคลื่อนไหว
            }
        }

        public void OnLook(InputValue value)
        {
            // ตรวจสอบว่าไม่ได้อยู่ในโหมด UI และ cursorInputForLook เป็นจริง ก่อนที่จะรับ Input การมอง
            if (cursorInputForLook && !isUIMode)
            {
                LookInput(value.Get<Vector2>());
            }
            else
            {
                LookInput(Vector2.zero); // หากอยู่ในโหมด UI ให้หยุดการมอง
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
                JumpInput(false); // หากอยู่ในโหมด UI ให้หยุดการกระโดด
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
                SprintInput(false); // หากอยู่ในโหมด UI ให้หยุด Sprint
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
                AttackInput(false); // หากอยู่ในโหมด UI ให้หยุดโจมตี
            }
        }

        // เพิ่มฟังก์ชันสำหรับรับ Input จากปุ่ม Esc
        public void OnEscape(InputValue value) // ต้องสร้าง Action "Escape" ใน Input Actions Asset ด้วย
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
            // เราจะควบคุม Cursor ผ่าน ToggleUIMode แทน
            // SetCursorState(cursorLocked); // คอมเมนต์หรือลบออก
            if (hasFocus && !isUIMode) // ถ้าแอปพลิเคชันได้รับ focus และไม่ได้อยู่ในโหมด UI ให้ล็อก Cursor
            {
                SetCursorState(true);
            }
            else if (!hasFocus && !isUIMode) // ถ้าเสีย focus และไม่ได้อยู่ในโหมด UI ให้ปลดล็อก
            {
                SetCursorState(false);
            }
            // ถ้าอยู่ในโหมด UI อยู่แล้ว (isUIMode เป็น true) ไม่ต้องทำอะไร เพราะ Cursor ถูกจัดการโดย ToggleUIMode แล้ว
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !newState; // ทำให้ Cursor แสดงเมื่อไม่ล็อก และซ่อนเมื่อล็อก
        }

        // ฟังก์ชันสำหรับสลับโหมด UI (เมาส์แสดง/หายไป, ตัวละครหยุด/เคลื่อนที่)
        public void ToggleUIMode()
        {
            isUIMode = !isUIMode; // สลับสถานะโหมด UI
            SetCursorState(!isUIMode); // ถ้าเป็นโหมด UI (true) ให้ปลดล็อก Cursor (false), ถ้าไม่ใช่ (false) ให้ล็อก Cursor (true)

            // รีเซ็ต Input ของตัวละครทันทีที่สลับโหมด เพื่อป้องกัน Input ค้าง
            move = Vector2.zero;
            look = Vector2.zero;
            jump = false;
            sprint = false;
            attack = false;

            // **Optional:** ถ้ามี UI Panel สำหรับ Pause Menu ให้เปิด/ปิดตรงนี้
            // if (pauseMenuPanel != null)
            // {
            //     pauseMenuPanel.SetActive(isUIMode);
            // }
        }
    }
}