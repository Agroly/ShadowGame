using UnityEngine.InputSystem;

namespace _project.Scripts.Input
{
            public class GameplayInput
            {
                private readonly InputActionMap _map;

                public readonly InputAction PrimaryContact;
                public readonly InputAction PrimaryPosition;
                public readonly InputAction SecondaryContact;
                public readonly InputAction SecondaryPosition;

                public GameplayInput()
                {
                    // Ищем карту Gameplay в ассете
                    _map = InputSystem.actions.FindActionMap("Gameplay");

                    PrimaryContact = _map.FindAction("PrimaryContact");
                    PrimaryPosition = _map.FindAction("PrimaryPosition");
                    SecondaryContact = _map.FindAction("SecondaryContact");
                    SecondaryPosition = _map.FindAction("SecondaryPosition");
                
                    _map.Enable(); 
                }
            }
}