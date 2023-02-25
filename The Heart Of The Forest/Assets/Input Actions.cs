// GENERATED AUTOMATICALLY FROM 'Assets/Input Actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input Actions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""9859d8c2-dc8e-48b3-a408-a310de0f92d0"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""659922a0-9caf-48d3-b5c6-294d1151b5ad"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1b2b7072-4477-44ee-9450-1c67b37127ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""1828fe0e-3b9d-47fc-95f4-4f4be3c56b58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability Burrow"",
                    ""type"": ""Button"",
                    ""id"": ""36a8deff-a53e-4c13-ba9f-93f7ba565df3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ability Glow"",
                    ""type"": ""Button"",
                    ""id"": ""43f640fa-4a33-469c-a7dd-ae2b75c1d63d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""430fa46f-27c4-4357-8ede-324261963401"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""90bccb4b-1bf1-46df-98e3-7eb06f16aa83"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AnyKey"",
                    ""type"": ""Button"",
                    ""id"": ""c32fe173-b072-4b36-b5b3-ec28b7b15653"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Map"",
                    ""type"": ""Button"",
                    ""id"": ""572cf769-35d1-4478-b25f-1ad2ed7c39af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pan Camera"",
                    ""type"": ""Value"",
                    ""id"": ""df8fb0af-dec7-42b0-95e7-b26b932ed36e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gui Input"",
                    ""type"": ""Value"",
                    ""id"": ""d038b937-4e86-48fb-82ac-0c404b2ec07f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle UI"",
                    ""type"": ""Button"",
                    ""id"": ""0a362e30-b84f-4906-a834-71b29e5640c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""307e29be-0490-4412-ae04-11b92bb77e9a"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""dd81e271-316f-4af9-be01-a9fe1647df29"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""50dd6145-9963-4852-8882-97d88b6c11c7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6874e0f6-82d6-4140-9e1b-96213da9b96c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Alt"",
                    ""id"": ""c573d605-690a-4329-975d-f524669b5151"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""762126f0-df49-4f12-b6ad-0312822e9fde"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""f075e65e-0210-4d36-ba4d-6e0fc2be3821"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Alt"",
                    ""id"": ""a8f1b5d2-10d3-4086-ba1e-956c0781d61e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""42b02b7c-4711-4f42-9d0b-b3c6f004330f"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""55903b86-4a6f-475b-85b7-271e02a44760"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""abffaf67-cd90-4b99-85cc-68ac5ccc06b1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e436f305-3cac-46ca-adb6-08758dfc6580"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""208d0b5d-4f92-4b45-8a01-b56a9d1b825b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f338994a-87fd-4507-804d-f65a56408621"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""307f3876-7b68-4eda-bef2-a14e28e74e87"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Ability Burrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad01183c-262e-4f49-bdcd-fafbf3b8285d"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ability Burrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e5258b6-b7e4-4ee6-9c3c-e8282f00161e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Ability Burrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""739efdfc-8012-4194-9000-d66a389de435"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ability Burrow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49a9533d-3d6b-4d0b-8d1b-f896f030609e"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Ability Glow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ad77f5b-bf36-42df-a058-adc2e20b3bc6"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ability Glow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edc374af-a0f8-4eb1-a8c2-bc483454aca2"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Ability Glow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e3b569e-56a4-4585-83f4-9feb50618f8f"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ability Glow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""198347d5-123a-4196-be91-b3aca4d64e84"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04bdb0c4-cb2d-4377-a0fc-57cc4f88e614"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff5b7495-ae55-4f48-a974-f43ce28bbe08"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f0b6f04-5ece-4ca1-ba13-32cb183e6f4d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c2f83e1-d037-4360-83e8-ab021de6cf64"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71193ed6-723d-43be-9aa6-4739521441f4"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08d55204-d6fe-4c29-8ed8-e380c5feaf5a"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65e4994d-52f7-4163-a121-1b719d41e116"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": ""AxisDeadzone(min=0.8,max=1)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""530d3a43-38bd-4cb7-a070-a81828344c79"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff98277f-a609-4b40-8460-dd4a23f0b3f6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53fcfbdb-9b03-4f98-b5ec-ac2243e0863e"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45849b22-ca65-4468-bfc5-b859a4fc6003"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""AnyKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9c1effe-904c-4629-a893-5e75e08e6884"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AnyKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b55cae59-71be-40a1-a21e-995eb11f7506"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ffae9c7-bb84-424d-9fdb-82d6e9a0e808"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75e6dbaa-ec58-41cf-9758-d352c587f27d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b711e78f-4fe1-4b8f-805d-bc7b516f9a4e"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbfad660-73d7-4221-8ee5-1bd7da9e02ef"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys [Keyboard]"",
                    ""id"": ""3dccaa64-e1dd-4220-b34d-2836980d9191"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2c5b2a1d-e435-4c37-b034-9af647efdf1c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""341191fb-7b0f-4cfb-98ab-c480d4f05e0f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""62ec28fe-a7d6-4d26-b836-891384a0ac9a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""36f9163d-0102-43d1-8b57-a672d99750be"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pan Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c4fedfc0-5e86-4298-b39b-b248a9397002"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Gui Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08b921b4-e536-4464-b2b9-ff4498f6b608"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Gui Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""742ee875-3725-4229-a60f-81aafe7b5f2c"",
                    ""path"": ""<Keyboard>/f4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle UI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""32d28650-e611-4ee0-abe2-c9f4976c5d3a"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3ddb53ac-92a2-45ee-a08e-6edd24c0ef15"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""4f3adde1-7bf1-41f1-9fda-8bfb55178c6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""f41f5621-9cb8-47bd-a3ea-c7cbae30fbfb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""3892a6ce-fde0-498c-9887-0affd8a53e07"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9e60104c-391e-4e21-8896-bc8afa195dc8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8ae97c8f-d4a2-4564-b669-143b1b169fd8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""704549ee-2559-40dc-9661-a77343de1862"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1cb7c144-7eec-43cc-ae77-6f7da338780c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""b7936761-453d-404c-8693-bb58fb93b98d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a58a48a5-1ab9-4489-a899-f47865a68ad4"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""858b8e50-cb0d-40fc-a733-f3f392b40281"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b1becfb6-d54a-4f69-8330-5223d09e8473"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""00e3e561-a9c0-4c47-a286-c63d7410f5d9"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""20721f11-bfb7-4cf3-9104-2e785638d497"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""812d35b6-abc1-4388-8ff6-b76fd986e738"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b97eb7b2-f1a7-4325-ab5e-6cd06bf6c256"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0ed62a04-95bf-4219-9888-6b4eff8e601a"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ff2059d7-c38c-4efb-ab59-3abb2fc80c1d"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""03fef4ea-29f9-4d61-92d6-712402fb218b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""03a90e03-f045-4fd9-96e0-403e0ec9bf04"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4246b325-8c2c-4099-b8f7-d3dfbb2d8c75"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""62eca697-bd37-4a3c-a203-2c890dd360cf"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""52998102-bc30-473e-b9dc-eb9b083df148"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3959f15e-8bb8-4c28-823e-760ecfff05c6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5f9506bb-4604-461e-a79a-d1fca2fa54f8"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""481389bf-3530-494b-bab8-57c8a222089a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4084b9b9-e617-4437-bd1e-1c6aa3f91f7d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""4390aaa4-3c4c-438d-bf1f-40aee5a65f52"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1b8280f-8cd6-4e6c-ab85-a1dbffcb4b8d"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf082524-be36-4dd6-9653-472851352546"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f719c337-2168-4114-ab71-e90d9ff743d4"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fcfb7f1-b799-473f-abef-1497951f7c63"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e178daed-4a21-4d11-9d40-fd2a7ead0d8b"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d058bf5f-93a6-4037-bcb5-29b342413348"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ce059b7-dc8b-4c3b-86e1-9d63fba320ad"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b607384-f613-4c5b-bcae-5e3608d74875"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Keyboard & Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Drop = m_Gameplay.FindAction("Drop", throwIfNotFound: true);
        m_Gameplay_AbilityBurrow = m_Gameplay.FindAction("Ability Burrow", throwIfNotFound: true);
        m_Gameplay_AbilityGlow = m_Gameplay.FindAction("Ability Glow", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        m_Gameplay_TogglePause = m_Gameplay.FindAction("TogglePause", throwIfNotFound: true);
        m_Gameplay_AnyKey = m_Gameplay.FindAction("AnyKey", throwIfNotFound: true);
        m_Gameplay_Map = m_Gameplay.FindAction("Map", throwIfNotFound: true);
        m_Gameplay_PanCamera = m_Gameplay.FindAction("Pan Camera", throwIfNotFound: true);
        m_Gameplay_GuiInput = m_Gameplay.FindAction("Gui Input", throwIfNotFound: true);
        m_Gameplay_ToggleUI = m_Gameplay.FindAction("Toggle UI", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
        m_UI_Click = m_UI.FindAction("Click", throwIfNotFound: true);
        m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
        m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Drop;
    private readonly InputAction m_Gameplay_AbilityBurrow;
    private readonly InputAction m_Gameplay_AbilityGlow;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_TogglePause;
    private readonly InputAction m_Gameplay_AnyKey;
    private readonly InputAction m_Gameplay_Map;
    private readonly InputAction m_Gameplay_PanCamera;
    private readonly InputAction m_Gameplay_GuiInput;
    private readonly InputAction m_Gameplay_ToggleUI;
    public struct GameplayActions
    {
        private @InputActions m_Wrapper;
        public GameplayActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Drop => m_Wrapper.m_Gameplay_Drop;
        public InputAction @AbilityBurrow => m_Wrapper.m_Gameplay_AbilityBurrow;
        public InputAction @AbilityGlow => m_Wrapper.m_Gameplay_AbilityGlow;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @TogglePause => m_Wrapper.m_Gameplay_TogglePause;
        public InputAction @AnyKey => m_Wrapper.m_Gameplay_AnyKey;
        public InputAction @Map => m_Wrapper.m_Gameplay_Map;
        public InputAction @PanCamera => m_Wrapper.m_Gameplay_PanCamera;
        public InputAction @GuiInput => m_Wrapper.m_Gameplay_GuiInput;
        public InputAction @ToggleUI => m_Wrapper.m_Gameplay_ToggleUI;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Drop.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDrop;
                @AbilityBurrow.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityBurrow;
                @AbilityBurrow.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityBurrow;
                @AbilityBurrow.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityBurrow;
                @AbilityGlow.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityGlow;
                @AbilityGlow.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityGlow;
                @AbilityGlow.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAbilityGlow;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @TogglePause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTogglePause;
                @TogglePause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTogglePause;
                @TogglePause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnTogglePause;
                @AnyKey.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAnyKey;
                @AnyKey.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAnyKey;
                @AnyKey.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAnyKey;
                @Map.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMap;
                @Map.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMap;
                @Map.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMap;
                @PanCamera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPanCamera;
                @PanCamera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPanCamera;
                @PanCamera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPanCamera;
                @GuiInput.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGuiInput;
                @GuiInput.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGuiInput;
                @GuiInput.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGuiInput;
                @ToggleUI.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleUI;
                @ToggleUI.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleUI;
                @ToggleUI.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnToggleUI;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @AbilityBurrow.started += instance.OnAbilityBurrow;
                @AbilityBurrow.performed += instance.OnAbilityBurrow;
                @AbilityBurrow.canceled += instance.OnAbilityBurrow;
                @AbilityGlow.started += instance.OnAbilityGlow;
                @AbilityGlow.performed += instance.OnAbilityGlow;
                @AbilityGlow.canceled += instance.OnAbilityGlow;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @TogglePause.started += instance.OnTogglePause;
                @TogglePause.performed += instance.OnTogglePause;
                @TogglePause.canceled += instance.OnTogglePause;
                @AnyKey.started += instance.OnAnyKey;
                @AnyKey.performed += instance.OnAnyKey;
                @AnyKey.canceled += instance.OnAnyKey;
                @Map.started += instance.OnMap;
                @Map.performed += instance.OnMap;
                @Map.canceled += instance.OnMap;
                @PanCamera.started += instance.OnPanCamera;
                @PanCamera.performed += instance.OnPanCamera;
                @PanCamera.canceled += instance.OnPanCamera;
                @GuiInput.started += instance.OnGuiInput;
                @GuiInput.performed += instance.OnGuiInput;
                @GuiInput.canceled += instance.OnGuiInput;
                @ToggleUI.started += instance.OnToggleUI;
                @ToggleUI.performed += instance.OnToggleUI;
                @ToggleUI.canceled += instance.OnToggleUI;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Point;
    private readonly InputAction m_UI_Click;
    private readonly InputAction m_UI_ScrollWheel;
    private readonly InputAction m_UI_MiddleClick;
    private readonly InputAction m_UI_RightClick;
    public struct UIActions
    {
        private @InputActions m_Wrapper;
        public UIActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Point => m_Wrapper.m_UI_Point;
        public InputAction @Click => m_Wrapper.m_UI_Click;
        public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
        public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                @Click.started -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnClick;
                @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDrop(InputAction.CallbackContext context);
        void OnAbilityBurrow(InputAction.CallbackContext context);
        void OnAbilityGlow(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnTogglePause(InputAction.CallbackContext context);
        void OnAnyKey(InputAction.CallbackContext context);
        void OnMap(InputAction.CallbackContext context);
        void OnPanCamera(InputAction.CallbackContext context);
        void OnGuiInput(InputAction.CallbackContext context);
        void OnToggleUI(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
}
