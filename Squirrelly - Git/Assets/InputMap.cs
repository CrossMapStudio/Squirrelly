// GENERATED AUTOMATICALLY FROM 'Assets/InputMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMap"",
    ""maps"": [
        {
            ""name"": ""MenuActions"",
            ""id"": ""d832b73c-28a6-4a89-b02a-328ce3e9e2f8"",
            ""actions"": [
                {
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""34ce96b1-5eef-4c0a-b394-b8054836fa55"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action2"",
                    ""type"": ""Button"",
                    ""id"": ""dbf613ff-aa76-49c0-a9eb-cc46641da0eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action3"",
                    ""type"": ""Button"",
                    ""id"": ""2fd61400-a74c-4f1c-be4c-454bd322a656"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action4"",
                    ""type"": ""Button"",
                    ""id"": ""beceba53-62b7-4d2d-b18d-ac6bf83cbb2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftStick"",
                    ""type"": ""Value"",
                    ""id"": ""800625b2-c9a7-473e-abf9-bcd9384a9b19"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadN"",
                    ""type"": ""Button"",
                    ""id"": ""d7273272-106b-4364-a873-92250c968fee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadS"",
                    ""type"": ""Button"",
                    ""id"": ""88a65589-05c6-41f4-92b1-e053f2069d64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadE"",
                    ""type"": ""Button"",
                    ""id"": ""bafe9f6b-1b55-4654-bad3-8f5f615fd033"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadW"",
                    ""type"": ""Button"",
                    ""id"": ""fa1ca787-95ed-4990-b6cb-015d616a3a8b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""fBumperRight"",
                    ""type"": ""Button"",
                    ""id"": ""43cd3b7d-71ca-42a7-95f9-3452c4979d0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""fBumperLeft"",
                    ""type"": ""Button"",
                    ""id"": ""3d1f91c9-eb0b-4b14-aad2-45ab7d40a5bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""bBumperRight"",
                    ""type"": ""Button"",
                    ""id"": ""3d17db4a-0fb6-4094-b8aa-014a19079a97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""bBumperLeft"",
                    ""type"": ""Button"",
                    ""id"": ""7ab16630-778e-4e3c-b200-d50c1791ea29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Options"",
                    ""type"": ""Button"",
                    ""id"": ""530226a6-fb37-4e1d-a5ec-91b38bd8d436"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2bb5fffc-6264-412f-ac6b-529134e0fadc"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45ca3712-466d-49e3-93d2-a46b2105b185"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""561c02ff-6402-4b86-aa27-a2e27d2a2a27"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bda3a4f-c327-45a6-b180-fe5d34913b77"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""743c3f2a-209a-4543-a515-7d86c3a57354"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftStick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c19a30f-bd13-4210-822c-19e1c62df1cf"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e6c6969-ce9a-4689-a1b8-b51147838a3f"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2373f28e-4ab1-407d-901d-d16f78582fa0"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadE"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f813fee9-cac7-42f5-9573-a5eefb7682a3"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadW"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8cd34882-c53b-4285-ace6-e0759cc24e9e"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""fBumperRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36470bc3-257f-4a32-8d1c-4d11a060c01d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""fBumperLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c74ff00-ec26-4a58-87d2-a14207be91aa"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""bBumperRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db856420-67e5-4841-90de-7c6f6e7182d3"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""bBumperLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f7e1130-3292-43d4-8d5d-5ca955e23748"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""In-Game"",
            ""id"": ""20e39856-5714-45cb-be5e-120b1a9b929f"",
            ""actions"": [
                {
                    ""name"": ""SouthAction"",
                    ""type"": ""Button"",
                    ""id"": ""400452d2-9955-458b-b6b6-dba7fc0e6171"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NorthAction"",
                    ""type"": ""Button"",
                    ""id"": ""b0facceb-f56d-48a7-afe3-721687ff48fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EastAction"",
                    ""type"": ""Button"",
                    ""id"": ""74e94950-81f5-4808-9d9e-d0451ac4ed2f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""WestAction"",
                    ""type"": ""Button"",
                    ""id"": ""887ba03f-ea64-41e3-9810-5f86ccd8e724"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftFrontBumper"",
                    ""type"": ""Button"",
                    ""id"": ""abe7e68e-e205-469b-84f4-4716b2e8f0f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftBackBumper"",
                    ""type"": ""Button"",
                    ""id"": ""9af65ecd-1a21-4787-948e-bf07280ec2a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightFrontBumper"",
                    ""type"": ""Button"",
                    ""id"": ""ac5cfebf-a16c-4f3f-a1d0-c9f06cd0565b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightBackBumper"",
                    ""type"": ""Button"",
                    ""id"": ""f69a8af6-85e2-4850-803b-6da4bbdb0828"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadSouth"",
                    ""type"": ""Button"",
                    ""id"": ""99fe82e1-84df-403a-9d1f-efba79a1cc5a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadNorth"",
                    ""type"": ""Button"",
                    ""id"": ""6b2e7cfd-cbc4-4706-bb6d-96af5633d175"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadEast"",
                    ""type"": ""Button"",
                    ""id"": ""858ee353-be6e-4928-a5e3-a95dea717d39"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DPadWest"",
                    ""type"": ""Button"",
                    ""id"": ""e5469acc-f956-402e-aba0-e9777eaec733"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Options"",
                    ""type"": ""Button"",
                    ""id"": ""15df676f-4313-41de-9303-5c751b985d32"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4c488315-7927-42c7-8c92-2ac6957be61b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SouthAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32376a68-f3bb-45f3-ba1b-4c54fcb9858b"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NorthAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7285d1f0-4bf8-4435-a2a0-33f72f3f3d32"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EastAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e326618-dd9f-4550-82f2-5a5153df4c32"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WestAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af406851-9e0c-4b59-8292-0062c5396c37"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d68a9e7-1ab9-413d-8d89-5f33ed1aa256"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25489957-fdd5-46d5-b514-3db8714d3058"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""908d9517-3248-4a89-95aa-9f22026ed904"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DPadWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96c4d501-e3d0-48b2-bf25-1112c74914da"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftBackBumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03b389df-ebcf-46d1-970f-52269e7df129"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightBackBumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c5137f6-34d0-4518-a949-bd124a4dd2bf"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftFrontBumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04f7b9a0-11a6-49b8-8cff-115a2c488a20"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightFrontBumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""51c1909a-1958-4c1d-9c26-91db810215e0"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Options"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MenuActions
        m_MenuActions = asset.FindActionMap("MenuActions", throwIfNotFound: true);
        m_MenuActions_Action1 = m_MenuActions.FindAction("Action1", throwIfNotFound: true);
        m_MenuActions_Action2 = m_MenuActions.FindAction("Action2", throwIfNotFound: true);
        m_MenuActions_Action3 = m_MenuActions.FindAction("Action3", throwIfNotFound: true);
        m_MenuActions_Action4 = m_MenuActions.FindAction("Action4", throwIfNotFound: true);
        m_MenuActions_LeftStick = m_MenuActions.FindAction("LeftStick", throwIfNotFound: true);
        m_MenuActions_DPadN = m_MenuActions.FindAction("DPadN", throwIfNotFound: true);
        m_MenuActions_DPadS = m_MenuActions.FindAction("DPadS", throwIfNotFound: true);
        m_MenuActions_DPadE = m_MenuActions.FindAction("DPadE", throwIfNotFound: true);
        m_MenuActions_DPadW = m_MenuActions.FindAction("DPadW", throwIfNotFound: true);
        m_MenuActions_fBumperRight = m_MenuActions.FindAction("fBumperRight", throwIfNotFound: true);
        m_MenuActions_fBumperLeft = m_MenuActions.FindAction("fBumperLeft", throwIfNotFound: true);
        m_MenuActions_bBumperRight = m_MenuActions.FindAction("bBumperRight", throwIfNotFound: true);
        m_MenuActions_bBumperLeft = m_MenuActions.FindAction("bBumperLeft", throwIfNotFound: true);
        m_MenuActions_Options = m_MenuActions.FindAction("Options", throwIfNotFound: true);
        // In-Game
        m_InGame = asset.FindActionMap("In-Game", throwIfNotFound: true);
        m_InGame_SouthAction = m_InGame.FindAction("SouthAction", throwIfNotFound: true);
        m_InGame_NorthAction = m_InGame.FindAction("NorthAction", throwIfNotFound: true);
        m_InGame_EastAction = m_InGame.FindAction("EastAction", throwIfNotFound: true);
        m_InGame_WestAction = m_InGame.FindAction("WestAction", throwIfNotFound: true);
        m_InGame_LeftFrontBumper = m_InGame.FindAction("LeftFrontBumper", throwIfNotFound: true);
        m_InGame_LeftBackBumper = m_InGame.FindAction("LeftBackBumper", throwIfNotFound: true);
        m_InGame_RightFrontBumper = m_InGame.FindAction("RightFrontBumper", throwIfNotFound: true);
        m_InGame_RightBackBumper = m_InGame.FindAction("RightBackBumper", throwIfNotFound: true);
        m_InGame_DPadSouth = m_InGame.FindAction("DPadSouth", throwIfNotFound: true);
        m_InGame_DPadNorth = m_InGame.FindAction("DPadNorth", throwIfNotFound: true);
        m_InGame_DPadEast = m_InGame.FindAction("DPadEast", throwIfNotFound: true);
        m_InGame_DPadWest = m_InGame.FindAction("DPadWest", throwIfNotFound: true);
        m_InGame_Options = m_InGame.FindAction("Options", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // MenuActions
    private readonly InputActionMap m_MenuActions;
    private IMenuActionsActions m_MenuActionsActionsCallbackInterface;
    private readonly InputAction m_MenuActions_Action1;
    private readonly InputAction m_MenuActions_Action2;
    private readonly InputAction m_MenuActions_Action3;
    private readonly InputAction m_MenuActions_Action4;
    private readonly InputAction m_MenuActions_LeftStick;
    private readonly InputAction m_MenuActions_DPadN;
    private readonly InputAction m_MenuActions_DPadS;
    private readonly InputAction m_MenuActions_DPadE;
    private readonly InputAction m_MenuActions_DPadW;
    private readonly InputAction m_MenuActions_fBumperRight;
    private readonly InputAction m_MenuActions_fBumperLeft;
    private readonly InputAction m_MenuActions_bBumperRight;
    private readonly InputAction m_MenuActions_bBumperLeft;
    private readonly InputAction m_MenuActions_Options;
    public struct MenuActionsActions
    {
        private @InputMap m_Wrapper;
        public MenuActionsActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Action1 => m_Wrapper.m_MenuActions_Action1;
        public InputAction @Action2 => m_Wrapper.m_MenuActions_Action2;
        public InputAction @Action3 => m_Wrapper.m_MenuActions_Action3;
        public InputAction @Action4 => m_Wrapper.m_MenuActions_Action4;
        public InputAction @LeftStick => m_Wrapper.m_MenuActions_LeftStick;
        public InputAction @DPadN => m_Wrapper.m_MenuActions_DPadN;
        public InputAction @DPadS => m_Wrapper.m_MenuActions_DPadS;
        public InputAction @DPadE => m_Wrapper.m_MenuActions_DPadE;
        public InputAction @DPadW => m_Wrapper.m_MenuActions_DPadW;
        public InputAction @fBumperRight => m_Wrapper.m_MenuActions_fBumperRight;
        public InputAction @fBumperLeft => m_Wrapper.m_MenuActions_fBumperLeft;
        public InputAction @bBumperRight => m_Wrapper.m_MenuActions_bBumperRight;
        public InputAction @bBumperLeft => m_Wrapper.m_MenuActions_bBumperLeft;
        public InputAction @Options => m_Wrapper.m_MenuActions_Options;
        public InputActionMap Get() { return m_Wrapper.m_MenuActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActionsActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActionsActions instance)
        {
            if (m_Wrapper.m_MenuActionsActionsCallbackInterface != null)
            {
                @Action1.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction1;
                @Action1.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction1;
                @Action1.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction1;
                @Action2.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction2;
                @Action2.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction2;
                @Action2.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction2;
                @Action3.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction3;
                @Action3.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction3;
                @Action3.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction3;
                @Action4.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction4;
                @Action4.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction4;
                @Action4.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnAction4;
                @LeftStick.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnLeftStick;
                @LeftStick.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnLeftStick;
                @LeftStick.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnLeftStick;
                @DPadN.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadN;
                @DPadN.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadN;
                @DPadN.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadN;
                @DPadS.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadS;
                @DPadS.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadS;
                @DPadS.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadS;
                @DPadE.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadE;
                @DPadE.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadE;
                @DPadE.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadE;
                @DPadW.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadW;
                @DPadW.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadW;
                @DPadW.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnDPadW;
                @fBumperRight.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnFBumperRight;
                @fBumperRight.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnFBumperRight;
                @fBumperRight.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnFBumperRight;
                @fBumperLeft.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnFBumperLeft;
                @fBumperLeft.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnFBumperLeft;
                @fBumperLeft.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnFBumperLeft;
                @bBumperRight.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnBBumperRight;
                @bBumperRight.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnBBumperRight;
                @bBumperRight.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnBBumperRight;
                @bBumperLeft.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnBBumperLeft;
                @bBumperLeft.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnBBumperLeft;
                @bBumperLeft.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnBBumperLeft;
                @Options.started -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnOptions;
                @Options.performed -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnOptions;
                @Options.canceled -= m_Wrapper.m_MenuActionsActionsCallbackInterface.OnOptions;
            }
            m_Wrapper.m_MenuActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Action1.started += instance.OnAction1;
                @Action1.performed += instance.OnAction1;
                @Action1.canceled += instance.OnAction1;
                @Action2.started += instance.OnAction2;
                @Action2.performed += instance.OnAction2;
                @Action2.canceled += instance.OnAction2;
                @Action3.started += instance.OnAction3;
                @Action3.performed += instance.OnAction3;
                @Action3.canceled += instance.OnAction3;
                @Action4.started += instance.OnAction4;
                @Action4.performed += instance.OnAction4;
                @Action4.canceled += instance.OnAction4;
                @LeftStick.started += instance.OnLeftStick;
                @LeftStick.performed += instance.OnLeftStick;
                @LeftStick.canceled += instance.OnLeftStick;
                @DPadN.started += instance.OnDPadN;
                @DPadN.performed += instance.OnDPadN;
                @DPadN.canceled += instance.OnDPadN;
                @DPadS.started += instance.OnDPadS;
                @DPadS.performed += instance.OnDPadS;
                @DPadS.canceled += instance.OnDPadS;
                @DPadE.started += instance.OnDPadE;
                @DPadE.performed += instance.OnDPadE;
                @DPadE.canceled += instance.OnDPadE;
                @DPadW.started += instance.OnDPadW;
                @DPadW.performed += instance.OnDPadW;
                @DPadW.canceled += instance.OnDPadW;
                @fBumperRight.started += instance.OnFBumperRight;
                @fBumperRight.performed += instance.OnFBumperRight;
                @fBumperRight.canceled += instance.OnFBumperRight;
                @fBumperLeft.started += instance.OnFBumperLeft;
                @fBumperLeft.performed += instance.OnFBumperLeft;
                @fBumperLeft.canceled += instance.OnFBumperLeft;
                @bBumperRight.started += instance.OnBBumperRight;
                @bBumperRight.performed += instance.OnBBumperRight;
                @bBumperRight.canceled += instance.OnBBumperRight;
                @bBumperLeft.started += instance.OnBBumperLeft;
                @bBumperLeft.performed += instance.OnBBumperLeft;
                @bBumperLeft.canceled += instance.OnBBumperLeft;
                @Options.started += instance.OnOptions;
                @Options.performed += instance.OnOptions;
                @Options.canceled += instance.OnOptions;
            }
        }
    }
    public MenuActionsActions @MenuActions => new MenuActionsActions(this);

    // In-Game
    private readonly InputActionMap m_InGame;
    private IInGameActions m_InGameActionsCallbackInterface;
    private readonly InputAction m_InGame_SouthAction;
    private readonly InputAction m_InGame_NorthAction;
    private readonly InputAction m_InGame_EastAction;
    private readonly InputAction m_InGame_WestAction;
    private readonly InputAction m_InGame_LeftFrontBumper;
    private readonly InputAction m_InGame_LeftBackBumper;
    private readonly InputAction m_InGame_RightFrontBumper;
    private readonly InputAction m_InGame_RightBackBumper;
    private readonly InputAction m_InGame_DPadSouth;
    private readonly InputAction m_InGame_DPadNorth;
    private readonly InputAction m_InGame_DPadEast;
    private readonly InputAction m_InGame_DPadWest;
    private readonly InputAction m_InGame_Options;
    public struct InGameActions
    {
        private @InputMap m_Wrapper;
        public InGameActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @SouthAction => m_Wrapper.m_InGame_SouthAction;
        public InputAction @NorthAction => m_Wrapper.m_InGame_NorthAction;
        public InputAction @EastAction => m_Wrapper.m_InGame_EastAction;
        public InputAction @WestAction => m_Wrapper.m_InGame_WestAction;
        public InputAction @LeftFrontBumper => m_Wrapper.m_InGame_LeftFrontBumper;
        public InputAction @LeftBackBumper => m_Wrapper.m_InGame_LeftBackBumper;
        public InputAction @RightFrontBumper => m_Wrapper.m_InGame_RightFrontBumper;
        public InputAction @RightBackBumper => m_Wrapper.m_InGame_RightBackBumper;
        public InputAction @DPadSouth => m_Wrapper.m_InGame_DPadSouth;
        public InputAction @DPadNorth => m_Wrapper.m_InGame_DPadNorth;
        public InputAction @DPadEast => m_Wrapper.m_InGame_DPadEast;
        public InputAction @DPadWest => m_Wrapper.m_InGame_DPadWest;
        public InputAction @Options => m_Wrapper.m_InGame_Options;
        public InputActionMap Get() { return m_Wrapper.m_InGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
        public void SetCallbacks(IInGameActions instance)
        {
            if (m_Wrapper.m_InGameActionsCallbackInterface != null)
            {
                @SouthAction.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnSouthAction;
                @SouthAction.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnSouthAction;
                @SouthAction.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnSouthAction;
                @NorthAction.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnNorthAction;
                @NorthAction.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnNorthAction;
                @NorthAction.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnNorthAction;
                @EastAction.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnEastAction;
                @EastAction.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnEastAction;
                @EastAction.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnEastAction;
                @WestAction.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnWestAction;
                @WestAction.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnWestAction;
                @WestAction.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnWestAction;
                @LeftFrontBumper.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnLeftFrontBumper;
                @LeftFrontBumper.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnLeftFrontBumper;
                @LeftFrontBumper.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnLeftFrontBumper;
                @LeftBackBumper.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnLeftBackBumper;
                @LeftBackBumper.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnLeftBackBumper;
                @LeftBackBumper.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnLeftBackBumper;
                @RightFrontBumper.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnRightFrontBumper;
                @RightFrontBumper.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnRightFrontBumper;
                @RightFrontBumper.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnRightFrontBumper;
                @RightBackBumper.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnRightBackBumper;
                @RightBackBumper.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnRightBackBumper;
                @RightBackBumper.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnRightBackBumper;
                @DPadSouth.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadSouth;
                @DPadSouth.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadSouth;
                @DPadSouth.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadSouth;
                @DPadNorth.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadNorth;
                @DPadNorth.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadNorth;
                @DPadNorth.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadNorth;
                @DPadEast.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadEast;
                @DPadEast.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadEast;
                @DPadEast.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadEast;
                @DPadWest.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadWest;
                @DPadWest.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadWest;
                @DPadWest.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnDPadWest;
                @Options.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnOptions;
                @Options.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnOptions;
                @Options.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnOptions;
            }
            m_Wrapper.m_InGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SouthAction.started += instance.OnSouthAction;
                @SouthAction.performed += instance.OnSouthAction;
                @SouthAction.canceled += instance.OnSouthAction;
                @NorthAction.started += instance.OnNorthAction;
                @NorthAction.performed += instance.OnNorthAction;
                @NorthAction.canceled += instance.OnNorthAction;
                @EastAction.started += instance.OnEastAction;
                @EastAction.performed += instance.OnEastAction;
                @EastAction.canceled += instance.OnEastAction;
                @WestAction.started += instance.OnWestAction;
                @WestAction.performed += instance.OnWestAction;
                @WestAction.canceled += instance.OnWestAction;
                @LeftFrontBumper.started += instance.OnLeftFrontBumper;
                @LeftFrontBumper.performed += instance.OnLeftFrontBumper;
                @LeftFrontBumper.canceled += instance.OnLeftFrontBumper;
                @LeftBackBumper.started += instance.OnLeftBackBumper;
                @LeftBackBumper.performed += instance.OnLeftBackBumper;
                @LeftBackBumper.canceled += instance.OnLeftBackBumper;
                @RightFrontBumper.started += instance.OnRightFrontBumper;
                @RightFrontBumper.performed += instance.OnRightFrontBumper;
                @RightFrontBumper.canceled += instance.OnRightFrontBumper;
                @RightBackBumper.started += instance.OnRightBackBumper;
                @RightBackBumper.performed += instance.OnRightBackBumper;
                @RightBackBumper.canceled += instance.OnRightBackBumper;
                @DPadSouth.started += instance.OnDPadSouth;
                @DPadSouth.performed += instance.OnDPadSouth;
                @DPadSouth.canceled += instance.OnDPadSouth;
                @DPadNorth.started += instance.OnDPadNorth;
                @DPadNorth.performed += instance.OnDPadNorth;
                @DPadNorth.canceled += instance.OnDPadNorth;
                @DPadEast.started += instance.OnDPadEast;
                @DPadEast.performed += instance.OnDPadEast;
                @DPadEast.canceled += instance.OnDPadEast;
                @DPadWest.started += instance.OnDPadWest;
                @DPadWest.performed += instance.OnDPadWest;
                @DPadWest.canceled += instance.OnDPadWest;
                @Options.started += instance.OnOptions;
                @Options.performed += instance.OnOptions;
                @Options.canceled += instance.OnOptions;
            }
        }
    }
    public InGameActions @InGame => new InGameActions(this);
    public interface IMenuActionsActions
    {
        void OnAction1(InputAction.CallbackContext context);
        void OnAction2(InputAction.CallbackContext context);
        void OnAction3(InputAction.CallbackContext context);
        void OnAction4(InputAction.CallbackContext context);
        void OnLeftStick(InputAction.CallbackContext context);
        void OnDPadN(InputAction.CallbackContext context);
        void OnDPadS(InputAction.CallbackContext context);
        void OnDPadE(InputAction.CallbackContext context);
        void OnDPadW(InputAction.CallbackContext context);
        void OnFBumperRight(InputAction.CallbackContext context);
        void OnFBumperLeft(InputAction.CallbackContext context);
        void OnBBumperRight(InputAction.CallbackContext context);
        void OnBBumperLeft(InputAction.CallbackContext context);
        void OnOptions(InputAction.CallbackContext context);
    }
    public interface IInGameActions
    {
        void OnSouthAction(InputAction.CallbackContext context);
        void OnNorthAction(InputAction.CallbackContext context);
        void OnEastAction(InputAction.CallbackContext context);
        void OnWestAction(InputAction.CallbackContext context);
        void OnLeftFrontBumper(InputAction.CallbackContext context);
        void OnLeftBackBumper(InputAction.CallbackContext context);
        void OnRightFrontBumper(InputAction.CallbackContext context);
        void OnRightBackBumper(InputAction.CallbackContext context);
        void OnDPadSouth(InputAction.CallbackContext context);
        void OnDPadNorth(InputAction.CallbackContext context);
        void OnDPadEast(InputAction.CallbackContext context);
        void OnDPadWest(InputAction.CallbackContext context);
        void OnOptions(InputAction.CallbackContext context);
    }
}
