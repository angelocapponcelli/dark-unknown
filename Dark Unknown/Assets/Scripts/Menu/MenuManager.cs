using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : Singleton<MenuManager>
    {
        private enum Menu
        {
            Main,
            Options,
            Audio,
            Graphics,
            Keybindings,
            Credits,
            Suggestions,
            Gamepad
        };
    
        public Animator animator;
        private static readonly int Quit = Animator.StringToHash("Quit");
    
        public GameObject MainMenu;
        public GameObject OptionsMenu;
        public GameObject AudioMenu;
        public GameObject GraphicsMenu;
        public GameObject KeybindingsMenu;
        public GameObject GamepadMenu;
        public GameObject CreditsMenu;
        public GameObject SuggestionsMenu;

        public Slider _playerVolumeSlider;
        public Slider _enemyVolumeSlider;
        public Slider _backgroundVolumeSlider;
        
        public Text move, dash, interact, potion, spell;
        public Tooltip toolTip;

        private Resolution[] _resolutions;
        public Dropdown resolutionDropdown;

        private GameObject[] _keybindingButtons;
        public static bool IsChangingKey;
        [SerializeField] private Texture2D customCursor;
    
        private UnityEngine.InputSystem.PlayerInput _playerInput;


        //generic function to activate a certain menu screen
        private void SetMenu(Menu menu)
        {
            MainMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            AudioMenu.SetActive(false);
            GraphicsMenu.SetActive(false);
            KeybindingsMenu.SetActive(false);
            GamepadMenu.SetActive(false);
            CreditsMenu.SetActive(false);
            SuggestionsMenu.SetActive(false);

            switch (menu)
            {
                case Menu.Main:
                    MainMenu.SetActive(true);
                    break;
                case Menu.Options:
                    OptionsMenu.SetActive(true);
                    break;
                case Menu.Audio:
                    AudioMenu.SetActive(true);
                    break;
                case Menu.Graphics:
                    GraphicsMenu.SetActive(true);
                    break;
                case Menu.Keybindings:
                    KeybindingsMenu.SetActive(true);
                    break;
                case Menu.Gamepad:
                    GamepadMenu.SetActive(true);
                    break;
                case Menu.Credits:
                    CreditsMenu.SetActive(true);
                    break;
                case Menu.Suggestions:
                    SuggestionsMenu.SetActive(true);
                    break;
            }
        }

        private void Start()
        {
            SetMenu(Menu.Main);

            _playerInput = InputManager.Instance.playerInput;
            _playerInput.camera = Camera.main;
            _playerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        
            _playerInput.SwitchCurrentActionMap("UI");
        
            var resolutions = Screen.resolutions.Select(resolution => new Resolution
            {
                width = resolution.width, height = resolution.height
            }).Distinct();
            _resolutions = resolutions as Resolution[] ?? resolutions.ToArray();
        
            resolutionDropdown.ClearOptions();
            var options = new List<string>();
            var currentResolutionIndex = 0;
            for (var i = 0; i < _resolutions.Length; i++)
            {
                var option = _resolutions[i].width + "x" + _resolutions[i].height;
                options.Add(option);
    
                if (_resolutions[i].width == Screen.currentResolution.width &&
                    _resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        
            _playerVolumeSlider.value = AudioManager.Instance.GetPlayerVolumeSound();
            _enemyVolumeSlider.value = AudioManager.Instance.GetEnemyVolumeSound();
            _backgroundVolumeSlider.value = AudioManager.Instance.GetBackgroundVolumeSound();
        }

        private void Update()
        {
            if ((AudioMenu.activeSelf ||
                 GraphicsMenu.activeSelf ||
                 GamepadMenu.activeSelf ||
                 (KeybindingsMenu.activeSelf && !IsChangingKey)) &&
                _playerInput.actions["Back"].WasPressedThisFrame())   //return to main menu
            {
                SetMenu(Menu.Options);
                InputManager.Instance.ResetIfNotSaved(_playerInput);
            }
            else if (KeybindingsMenu.activeSelf && IsChangingKey)
            {
            }
            else if (_playerInput.actions["Back"].WasPressedThisFrame())
            {
                SetMenu(Menu.Main);
            }
        }
        
        public void UpdateText(UnityEngine.InputSystem.PlayerInput player)
        {
            move.text = player.actions["Move"].bindings[1].ToDisplayString() + "/" +
                        player.actions["Move"].bindings[2].ToDisplayString() + "/" +
                        player.actions["Move"].bindings[3].ToDisplayString() + "/" +
                        player.actions["Move"].bindings[4].ToDisplayString();
            dash.text = player.actions["Dash"].bindings[0].ToDisplayString();
            interact.text = player.actions["Interact"].bindings[0].ToDisplayString();
            potion.text = player.actions["Potion"].bindings[0].ToDisplayString();
            spell.text = player.actions["Spell"].bindings[0].ToDisplayString();
        }

        public void OpenNotification()
        { 
            toolTip.StartOpen();
        }
    
        //reactors to the pressing of a button
        public void OpenMainMenu()
        {
            SetMenu(Menu.Main);
        }
    
        public void OpenOptionsMenu()
        {
            SetMenu(Menu.Options);
        }
    
        public void OpenAudioMenu()
        {
            SetMenu(Menu.Audio);
        }
    
        public void OpenGraphicsMenu()
        {
            SetMenu(Menu.Graphics);
        }
    
        public void OpenKeybindingsMenu()
        {
            SetMenu(Menu.Keybindings);
        }
        
        public void OpenGamepadMenu()
        {
            SetMenu(Menu.Gamepad);
        }

        public void OpenCreditsMenu()
        {
            SetMenu(Menu.Credits);
        }
    
        public void OpenSuggestionsMenu()
        {
            SetMenu(Menu.Suggestions);
        }
    
        public void Submit()
        {
            SetMenu(Menu.Main);
        }

        public void PlayGame()
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.ForceSoftware);
            InputManager.Instance.playerInput.SwitchCurrentActionMap("Gameplay");
            animator.SetTrigger(Quit);
            StartCoroutine(LoadGameCoroutine());

            AudioManager.Instance.PlaySoundTrackIntro();
        }
    
        private static IEnumerator LoadGameCoroutine()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("InitialCutscene");
        }

        public void QuitGame()
        {
            animator.SetTrigger(Quit);
            Debug.Log("Quit game...");
            StartCoroutine(QuitCoroutine());
        }
    
        private static IEnumerator QuitCoroutine()
        {
            yield return new WaitForSeconds(1);
            Application.Quit();
        }

        //UI AUDIO
        public void PlayOverUIButtonSound()
        {
            AudioManager.Instance.PlayOverUIButtonSound();
        }
        public void PlayClickUIButtonSound()
        {
            AudioManager.Instance.PlayClickUIButtonSound();
        }

        //OPTION SETTINGS
        public void SetBackgroundVolume(float value)
        {
            AudioManager.Instance.SetBackgroundVolume(value);
        }
        public void SetPlayerVolume(float value)
        {
            AudioManager.Instance.SetPlayerVolume(value);
        }
        public void SetEnemyVolume(float value)
        {
            AudioManager.Instance.SetEnemyVolume(value);
        }

        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            var resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

    }
}
