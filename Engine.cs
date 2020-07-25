using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///<summary>
public class Engine : PCO
{   

    // User options>>
    public float ParkingBrake_Velocity = 0.5f;
    public float ParkingBrake_Lag = 0.5f; //the time will spend for the brake to work
    public float Torque = 2000.0f;
    public float Max_Speed = 8.0f;
    public float Turn_Brake_Drag = 150.0f;
    public float MaxAngVelocity_Limit = 45.0f;
    public float Pivot_Turn_Rate = 0.3f; // speed turn around pivot

    public bool Acceleration_Flag = false;
    public float Acceleration_Time = 4.0f;
    public float Deceleration_Time = 0.1f;

    public bool Torque_Limitter = false;
    public float Max_Slope_Angle = 45.0f;

    public bool Use_AntiSlip = false;
    public float Ray_Distance = 1.0f;
    // << User options

    // Set by ProcessInput()
    bool Stop_Flag = true; // Referred to from "Steer_Wheel_CS".
    float L_Input_Rate;
    float R_Input_Rate;
    float Turn_Brake_Rate; //Make the tank stop turnning automatacilly

    // Referred to from Track.
    public float Speed_Rate; // Referred to from also "Input_Type_Settings_CS".
    public float L_Brake_Drag;
    public float R_Brake_Drag;
    public float Left_Torque;
    public float Right_Torque;

    //flags>>
    public bool Parking_Brake;
    

    //privates>>
    Rigidbody rb;

    float leftSpeedRate;
    float rightSpeedRate;
    float defaultTorque;
    float acceleRate;
    float deceleRate;
    float stoppingTime; //the time took for the tank to stop

    public override void ProcessInput()
    {   
        float vertical=0.0f;
		float horizontal=0.0f;
		float brakingTime = 1.0f;
        // Set "vertical".
        if (Input.GetKeyDown (KeyCode.W)) { // Forward
            vertical += 0.25f;
        } else if (Input.GetKeyDown (KeyCode.S)) { // Backward
            vertical -= 0.25f;
        } else if (Input.GetKey (KeyCode.Space)) { // Stop
            vertical = 0.0f;
        }
        vertical = Mathf.Clamp (vertical, -0.5f, 1.0f);

        // Set "horizontal".
        if (Input.GetKey(KeyCode.A)) { // Left
            horizontal = -1.0f;
        } else if (Input.GetKey(KeyCode.D)) { // Right
            horizontal = 1.0f;
        } else { // No turn.
            horizontal = 0.0f;
        }

        //Set flags
        if (vertical == 0.0f && horizontal == 0.0f) { // The tank should stop.
            Stop_Flag = true;
            L_Input_Rate = 0.0f;
            R_Input_Rate = 0.0f;
            Turn_Brake_Rate = 0.0f;
			return;	
        }
        else { // The tank should be driving.
            Stop_Flag = false;
        }

        // In case of going straight.
        if (horizontal == 0.0f) { // The tank should be going straight.
            L_Input_Rate = -vertical;
            R_Input_Rate = vertical;
            Turn_Brake_Rate = 0.0f;
            return;
        }

        // In case of pivot-turn.
        if (vertical == 0.0f) { // The tank should be doing pivot-turn.
            horizontal *= Pivot_Turn_Rate;
            L_Input_Rate = -horizontal;
            R_Input_Rate = -horizontal;
            Turn_Brake_Rate = 0.0f;
            return;
        }


        if (horizontal < 0.0f) { // Left turn.
            L_Input_Rate = 0.0f;
            R_Input_Rate = vertical;
        }
        else { // Right turn.
            L_Input_Rate = -vertical;
            R_Input_Rate = 0.0f;
        }

        // Increase the "Turn_Brake_Rate" with the lapse of time.
        Turn_Brake_Rate += (1.0f / brakingTime / Mathf.Abs (Speed_Rate)) * Time.deltaTime * Mathf.Sign(horizontal);
        Turn_Brake_Rate = Mathf.Clamp (Turn_Brake_Rate, -1.0f, 1.0f);

    }

   

    
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    void init(){
        rb=GetComponent<Rigidbody>();

        defaultTorque = Torque;
        if (Acceleration_Flag) {
            acceleRate = 1.0f / Acceleration_Time;
            deceleRate = 1.0f / Deceleration_Time;
        }
    }

    void Update(){

		ProcessInput();
        // Set "leftSpeedRate" and "rightSpeedRate".
        Set_Speed_Rate();
    }

    
    void Set_Speed_Rate (){
        // Set the "leftSpeedRate" and "rightSpeedRate".
        if (Acceleration_Flag) {
            Acceleration_And_Deceleration();
        }
        else {
            Constant_Speed();
        }

        // Set the "Speed_Rate" value.
        if (Mathf.Abs (leftSpeedRate) > Mathf.Abs (rightSpeedRate)) {
            Speed_Rate = Mathf.Abs (leftSpeedRate);
        }
        else {
            Speed_Rate = Mathf.Abs (rightSpeedRate);
        }

        // Set the "L_Brake_Drag" and "R_Brake_Drag".
        L_Brake_Drag = Mathf.Clamp (Turn_Brake_Drag * -Turn_Brake_Rate, 0.0f, Turn_Brake_Drag);
        R_Brake_Drag = Mathf.Clamp (Turn_Brake_Drag * Turn_Brake_Rate, 0.0f, Turn_Brake_Drag);

        // Set the "Left_Torque" and "Right_Torque".
        Left_Torque = Torque * -Mathf.Sign(leftSpeedRate) * Mathf.Ceil (Mathf.Abs (leftSpeedRate)); // (Note.) When the "leftSpeedRate" is zero, the torque will be set to zero.
        Right_Torque = Torque * Mathf.Sign(rightSpeedRate) * Mathf.Ceil (Mathf.Abs (rightSpeedRate));
    }
    void Constant_Speed ()
    {
        leftSpeedRate = -L_Input_Rate;
        rightSpeedRate = R_Input_Rate;
    }

    void Acceleration_And_Deceleration ()
    {
        // Synchronize the left and right speed rates to increase the straightness.
        if (Stop_Flag == false && L_Input_Rate == -R_Input_Rate) { // Not stopping && Going straight.
            // Set the average value to the both sides.
            float averageRate = (leftSpeedRate + rightSpeedRate) * 0.5f;
            leftSpeedRate = averageRate;
            rightSpeedRate = averageRate;
        }

        // Set the speed rates.
        leftSpeedRate = Calculate_Speed_Rate (leftSpeedRate, -L_Input_Rate);
        rightSpeedRate = Calculate_Speed_Rate (rightSpeedRate, R_Input_Rate);
    }

    float Calculate_Speed_Rate (float currentRate, float targetRate)
		{
			float moveRate;
			if (Mathf.Sign (targetRate) == Mathf.Sign (currentRate)) { // The both rates have the same direction.
				if (Mathf.Abs (targetRate) > Mathf.Abs (currentRate)) { // It should be in acceleration.
					moveRate = acceleRate;
				} else { // It should be in deceleration.
					moveRate = deceleRate;
				}
			} else { // The both rates have different directions. >> It should be in deceleration until the currentRate becomes zero.
				moveRate = deceleRate;
			}
			return Mathf.MoveTowards (currentRate, targetRate, moveRate * Time.deltaTime);
		}



    void FixedUpdate(){

        // Control the automatic parking brake.
        Control_Parking_Brake ();
        
        // Call anti-spinning function for high speed tanks.
        Anti_Spin ();

        // Call anti-slipping function.
        if (Use_AntiSlip) {
            Anti_Slip ();
        }

        // Limit the torque in slope.
        if (Torque_Limitter) {
            Limit_Torque ();
        }
    }

    void Control_Parking_Brake ()
    {
        if (Stop_Flag) { // The tank should stop.
				// Get the velocities of the Rigidbody.
				float currentVelocity = rb.velocity.magnitude;
				float currentAngularVelocity = rb.angularVelocity.magnitude;

				// 
				if (Parking_Brake == true) { // The parking brake is working now.
					// Check the Rigidbody velocities.
					if (currentVelocity > ParkingBrake_Velocity && currentAngularVelocity > ParkingBrake_Velocity) { // The Rigidbody should have been moving by receiving external force.
						// Release the parking brake.
						Parking_Brake = false;
						rb.constraints = RigidbodyConstraints.None;
						stoppingTime = 0.0f;
						return;
					} // The Rigidbody almost stops.
					return;
				}
				else { // The parking brake is not working.
					// Check the Rigidbody velocities.
					if (currentVelocity < ParkingBrake_Velocity && currentAngularVelocity < ParkingBrake_Velocity) { // The Rigidbody almost stops.
						// Count the stopping time.
						stoppingTime += Time.fixedDeltaTime;
						if (stoppingTime > ParkingBrake_Lag) { // The stopping time has been over the "ParkingBrake_Lag".
							// Put on the parking brake.
							Parking_Brake = true;
							rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY;
							leftSpeedRate = 0.0f;
							rightSpeedRate = 0.0f;
							return;
						}
						else { // The stopping time has not over yet.
							return;
						}
					} // The Rigidbody almost stops.
					return;
				}
			}
			else { // The tank should be driving now.
				if (Parking_Brake == true) { // The parking brake is still working.
					// Release parking brake.
					Parking_Brake = false;
					rb.constraints = RigidbodyConstraints.None;
					stoppingTime = 0.0f;
				}
			}
    }

    void Anti_Spin ()
    {
        // Reduce the spinning motion by controling the angular velocity of the Rigidbody.
        if (L_Input_Rate != R_Input_Rate && Turn_Brake_Rate == 0.0f) { // The tank should not be doing pivot-turn or brake-turn.
            // Control the angular velocity of the Rigidbody.
            Vector3 currentAngularVelocity = rb.angularVelocity;
            currentAngularVelocity.y *= 0.9f; // Reduce the angular velocity on Y-axis.
            // Set the new angular velocity.
            rb.angularVelocity = currentAngularVelocity;
        }
    }

    Ray ray = new Ray ();
    int layerMask = ~((1 << 10) + (1 << 2) + (1 << 11)); // Layer 2 = Ignore Ray, Layer 10 = Ignore All,  Layer 11 = MainBody.
    void Anti_Slip ()
    {
        // Reduce the slippage by controling the velocity of the Rigidbody.
        // Cast the ray downward to detect the ground.
        ray.origin = transform.position;
        ray.direction = -transform.up;
        if (Physics.Raycast(ray, Ray_Distance, layerMask) == true) { // The ray hits the ground.
            // Control the velocity of the Rigidbody.
            Vector3 currentVelocity = rb.velocity;
            if (leftSpeedRate == 0.0f && rightSpeedRate == 0.0f) { // The tank should stop.
                // Reduce the Rigidbody velocity gradually.
                currentVelocity.x *= 0.9f;
                currentVelocity.z *= 0.9f;
            } else { // The tank should been driving.
                float sign;
                if (leftSpeedRate == rightSpeedRate) { // The tank should be going straight forward or backward.
                    sign = Mathf.Sign(leftSpeedRate);
                }
                else if (leftSpeedRate == -rightSpeedRate) { // The tank should be doing pivot-turn.
                    sign = 1.0f;
                }
                else { // The tank should be doing brake-turn.
                    sign = Mathf.Sign(leftSpeedRate + rightSpeedRate);
                }
                // Change the velocity of the Rigidbody forcibly.
                currentVelocity = Vector3.MoveTowards(currentVelocity, transform.forward * sign * currentVelocity.magnitude, 50.0f * Time.fixedDeltaTime);
            }
            // Set the new velocity.
            rb.velocity = currentVelocity;
        }
    }

    void Limit_Torque ()
		{
			// Reduce the torque according to the angle of the slope.
			float torqueRate = Mathf.DeltaAngle (transform.eulerAngles.x, 0.0f) / Max_Slope_Angle;
			if (leftSpeedRate > 0.0f && rightSpeedRate > 0.0f) { // The tank should be going forward.
				Torque = Mathf.Lerp (defaultTorque, 0.0f, torqueRate);
			} else { // The tank should be going backward.
				Torque = Mathf.Lerp (defaultTorque, 0.0f, -torqueRate);
			}
		}

}
