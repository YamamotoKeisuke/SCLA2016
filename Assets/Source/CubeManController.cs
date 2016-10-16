using UnityEngine;
using RootSystem = System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Kinect2 = Windows.Kinect;
using System.Linq;

public class CubeManController : MonoBehaviour {

    //Assignments for a bitmask to control which bones to look at and which to ignore
    public enum BoneMask
    {
        None = 0x0,
        //EMPTY = 0x1,
        Spine = 0x2,
        Shoulder_Center = 0x4,
        Head = 0x8,
        Shoulder_Left = 0x10,
        Elbow_Left = 0x20,
        Wrist_Left = 0x40,
        Hand_Left = 0x80,
        Shoulder_Right = 0x100,
        Elbow_Right = 0x200,
        Wrist_Right = 0x400,
        Hand_Right = 0x800,
        Hips = 0x1000,
        Knee_Left = 0x2000,
        Ankle_Left = 0x4000,
        Foot_Left = 0x8000,
        //EMPTY = 0x10000,
        Knee_Right = 0x20000,
        Ankle_Right = 0x40000,
        Foot_Right = 0x80000,
        All = 0xEFFFE,
        Torso = 0x1000000 | Spine | Shoulder_Center | Head, //the leading bit is used to force the ordering in the editor
        Left_Arm = 0x1000000 | Shoulder_Left | Elbow_Left | Wrist_Left | Hand_Left,
        Right_Arm = 0x1000000 | Shoulder_Right | Elbow_Right | Wrist_Right | Hand_Right,
        Left_Leg = 0x1000000 | Hips | Knee_Left | Ankle_Left | Foot_Left,
        Right_Leg = 0x1000000 | Hips | Knee_Right | Ankle_Right | Foot_Right,
        R_Arm_Chest = Right_Arm | Spine,
        No_Feet = All & ~(Foot_Left | Foot_Right),
        Upper_Body = Head | Elbow_Left | Wrist_Left | Hand_Left | Elbow_Right | Wrist_Right | Hand_Right
    }

    private Kinect2.KinectSensor _Sensor;
    private Kinect2.BodyFrameReader _Reader;
    private Kinect2.Body[] _Bodies;
    private Kinect2.Body _Body;

    public bool bDebugLog = true;


    public GameObject Root;
    public GameObject Head;
    public GameObject HandLeft;
    public GameObject HandRight;
    public GameObject KneeLeft;
    public GameObject KneeRight;
    public GameObject ShoulderLeft;
    public GameObject ShoulderRight;

    public int player;
    public BoneMask Mask = BoneMask.All;
    public bool animated = true;
    public float moveWeight = 0.2f;

    private GameObject[] _bones; //internal handle for the bones of the model
    private Vector3[] _vecbones;
    private Vector3[] _vecbones2;

    //private uint _nullMask = 0x0;

    private Quaternion[] _baseRotation; //starting orientation of the joints
    private Vector3[] _boneDir; //in the bone's local space, the direction of the bones
    private Vector3[] _boneUp; //in the bone's local space, the up vector of the bone

    public const int POSCORRECTIONX = 5;
    public const int POSCORRECTIONY = 5;

    public GameObject gimmick;

    bool flag;

	// Use this for initialization
	void Start () 
    {
        _bones = new GameObject[7]{
            Head, HandLeft, HandRight, KneeLeft, KneeRight, ShoulderLeft, ShoulderRight
        };

        _Sensor = Kinect2.KinectSensor.GetDefault();
        
        if(_Sensor != null)
        {
            if(!_Sensor.IsOpen)
            {
                if (bDebugLog) Debug.Log("[Kinect2] KinectSensor Open");
                _Sensor.Open();

                // ボディリーダーを開く
                _Reader = _Sensor.BodyFrameSource.OpenReader();
                _Reader.FrameArrived += BodyFrameReader_FrameArrived;

            }
        }

        //gimmick = GameObject.Find("ita");
        flag = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(_Body != null)
        {
            if (_Body.IsTracked) 
            {
                /*foreach (Kinect2.Body body in _Bodies)
                {
                    if(body.IsTracked)
                    {
                        for(int joint = 0; joint < (int)Kinect2.JointType.ThumbRight; joint++)
                        {
                            if(joint == (int)Kinect2.JointType.Head)
                            {
                                //Debug.Log("Head");
                                _bones[joint].transform.position.x = 

                            }
                            if (joint == (int)Kinect2.JointType.HandLeft)
                            {
                                //Debug.Log("HandLeft");
                            }
                            if (joint == (int)Kinect2.JointType.HandRight)
                            {
                                //Debug.Log("HandRight");
                            }
                            if (joint == (int)Kinect2.JointType.KneeLeft)
                            {
                                //Debug.Log("KneeLeft");
                            }
                            if (joint == (int)Kinect2.JointType.KneeRight)
                            {
                                //Debug.Log("KneeRight");
                            }
                        }

                    }
                }*/

                
                foreach(var body in _Bodies.Where(b => b.IsTracked))
                {
                    foreach(var joint in body.Joints)
                    {
                        //var point = _Sensor.CoordinateMapper.MapCameraPointToDepthSpace(joint.Value.Position);
                        


                        // 手の位置が追跡状態
                        if(joint.Value.TrackingState == Kinect2.TrackingState.Tracked)
                        {
                            //Debug.Log("手を追跡中");

                            // 左手を追跡
                            if(joint.Value.JointType == Kinect2.JointType.HandLeft)
                            {
                                //Debug.Log("左手追跡中");
                                _bones[1].transform.position = new Vector3(
                                                                    joint.Value.Position.X*POSCORRECTIONX, 
                                                                    joint.Value.Position.Y*POSCORRECTIONY, 
                                                                    joint.Value.Position.Z
                                                                );

                            }

                            // 右手を追跡
                            if (joint.Value.JointType == Kinect2.JointType.HandRight)
                            {
                                //Debug.Log("右手追跡中");
                                _bones[2].transform.position = new Vector3(
                                                                    joint.Value.Position.X *POSCORRECTIONX,
                                                                    joint.Value.Position.Y * POSCORRECTIONY,
                                                                    joint.Value.Position.Z
                                                                );

                            }

                            // 左ひざを追跡
                            if (joint.Value.JointType == Kinect2.JointType.KneeLeft)
                            {
                                //Debug.Log("左ひざ追跡中");
                                _bones[3].transform.position = new Vector3(
                                                                    joint.Value.Position.X * POSCORRECTIONX,
                                                                    joint.Value.Position.Y * POSCORRECTIONY,
                                                                    joint.Value.Position.Z
                                                                );

                            }

                            // 右ひざを追跡
                            if (joint.Value.JointType == Kinect2.JointType.KneeRight)
                            {
                                //Debug.Log("右ひざ追跡中");
                                _bones[4].transform.position = new Vector3(
                                                                    joint.Value.Position.X * POSCORRECTIONX,
                                                                    joint.Value.Position.Y * POSCORRECTIONY,
                                                                    joint.Value.Position.Z
                                                                );

                            }

                            // 頭を追跡
                            if (joint.Value.JointType == Kinect2.JointType.Head)
                            {
                                //Debug.Log("左手追跡中");
                                _bones[0].transform.position = new Vector3(
                                                                    joint.Value.Position.X * POSCORRECTIONX,
                                                                    joint.Value.Position.Y * POSCORRECTIONY,
                                                                    joint.Value.Position.Z
                                                                );

                            }

                            // 左肩を追跡
                            if (joint.Value.JointType == Kinect2.JointType.ShoulderLeft)
                            {
                                
                                _bones[5].transform.position = new Vector3(
                                                                    joint.Value.Position.X * POSCORRECTIONX,
                                                                    joint.Value.Position.Y * POSCORRECTIONY,
                                                                    joint.Value.Position.Z
                                                                );

                            }

                            // 右肩を追跡
                            if (joint.Value.JointType == Kinect2.JointType.ShoulderRight)
                            {

                                _bones[6].transform.position = new Vector3(
                                                                    joint.Value.Position.X * POSCORRECTIONX,
                                                                    joint.Value.Position.Y * POSCORRECTIONY,
                                                                    joint.Value.Position.Z
                                                                );

                            }

                        }
                        

                        /*
                        // 手の位置が推測状態
                        else if (joint.Value.TrackingState == Kinect2.TrackingState.Inferred)
                        {
                            //Debug.Log("手の位置推測中");
                        }*/
                    }
                }
            }
        }

        if(_bones[1].transform.position.x < _bones[5].transform.position.x
            && _bones[2].transform.position.x < _bones[5].transform.position.x
            && !flag
            )
        {
            flag = true;
        }

        if (_bones[1].transform.position.x > _bones[6].transform.position.x
            && _bones[2].transform.position.x > _bones[6].transform.position.x
            && flag
            )
        {
            gimmick.transform.position += new Vector3(0, 0, -1f);
            flag = false;
        }

	}

    void BodyFrameReader_FrameArrived(object sender, Kinect2.BodyFrameArrivedEventArgs e)
    {
        var reference = e.FrameReference.AcquireFrame();
        using (var frame = reference)
        {
            if (frame != null)
            {
                // Bodyを入れる配列を作る
                _Bodies = new Kinect2.Body[frame.BodyFrameSource.BodyCount];

                // ボディデータを取得
                frame.GetAndRefreshBodyData(_Bodies);

                foreach (Kinect2.Body body in _Bodies)
                {
                    if (body.IsTracked)
                    {
                        _Body = body;
                    }
                }
                frame.Dispose();

            }
        }
    }



}
