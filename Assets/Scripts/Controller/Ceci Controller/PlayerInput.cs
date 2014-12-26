using System;
using UnityEngine;
public class PlayerInput : MonoBehaviour
{
    private MovementController controller;
    void Awake()
    {
        SpawnManager.PlayerTransform = this.gameObject.transform;
        controller = GetComponent<MovementController>();
    }
    void Update()
    {
        // Left and Right Stuff
		int rawH = RebindableInput.GetAxis("Horizontal");
        controller.Right = rawH > 0;
        controller.Left = rawH < 0;
        if(controller.Right || controller.Left) // check needed in case standing still
        {
            controller.isFacingRight = controller.Right;
        }
        
        // Ladder Stuff
        int rawV = RebindableInput.GetAxis("Vertical");
        controller.Up = rawV > 0;
        controller.Down = rawV < 0;
        
        // Jumping Stuff
		//bool pressJump = RebindableInput.GetKeyDown("Jump");
		controller.Up |= RebindableInput.GetKey("Jump");
		controller.UpPress = controller.Up && !controller.PrevUp;// || pressJump;
        controller.UpHold = controller.Up && (controller.PrevUp == controller.Up);
        controller.UpRelease = !controller.Up && controller.PrevUp;
		controller.PrevUp = controller.Up;
    }
}

