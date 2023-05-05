using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManagers : MonoBehaviour
{
    public GameObject damegeTextPrefab;
    public GameObject healthTextPrefab;
    // Start is called before the first frame update
    public Canvas gameCanavas;
    private void Awake()
    {
        gameCanavas = FindAnyObjectByType<Canvas>();
       
    }

    public void CharacterTookDamage(GameObject character, int damageReceied)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tMP_Text = Instantiate(damegeTextPrefab, spawnPosition, Quaternion.identity, gameCanavas.transform)
            .GetComponent<TMP_Text>(); 
        tMP_Text.text = damageReceied.ToString(); 
    }
    public void OnEnable()
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealth += CharacterHealth;
    }

    public void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealth -= CharacterHealth;
    }
    public void CharacterHealth(GameObject character, int health)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tMP_Text = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanavas.transform)
            .GetComponent<TMP_Text>();
        tMP_Text.text = health.ToString();
    }
    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
#if(UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
    Application.Quit();
#elif (UNITY_WEBGL)
    SceneManager.LoadGame("QuitScene");
#endif   

        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
