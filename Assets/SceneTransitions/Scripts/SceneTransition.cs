using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject player;

    public Animator animator;

    [SerializeField] private PersistentDataSO persistentDataSO;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private GameObject sword;

    [SerializeField] private string scene;

    float transitionTime = 1f;

    private void Start()
    {
        sword = GameObject.FindGameObjectWithTag("FloatingSword");
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        if (levelManager == LevelManager.ActiveLevels)
        {
            player.transform.position = spawnPoint.position;
            if (persistentDataSO.hasSword)
            {
                sword.transform.position = spawnPoint.position;
            }
        }
    }

    public Vector2 GetSpawnPointPosition()
    {
        return spawnPoint.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            LevelManager.ActiveLevels = levelManager;
            playerController.LockMovement();
            playerController.dodgeSpd = 0;
            LoadScene(scene);
        }
    }

    private void LoadScene(string sceneName)
    {
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(LoadLevel(sceneName));
    }
    
    IEnumerator LoadLevel(string sceneName)
    {
        animator.SetTrigger("Start");
        playerController.animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
