using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] TouchInputManager touchInputManager;
    [SerializeField] FruitSpawnner spawnner;
    [SerializeField] Transform fruitSpawnPoint;
    [SerializeField] Cloud cloud;
    [SerializeField] EffectController effectController;
    [SerializeField] CoinController coinController;
    [SerializeField] float delaySpawnTime;
    [SerializeField] float delayDestroyAllFruits,delayStartDestroyAllFruit;
    bool canSpawnFruit = true;
    float lastSpawnTime;
    bool isUsingSkill = false;
    private void Awake()
    {
        touchInputManager.OnBeginDragEvent += OnBeginDrag;
        touchInputManager.OnDragEvent += OnDrag;
        touchInputManager.OnEndDragEvent += OnEndDrag;
        lastSpawnTime = Time.time;
    }
    private void Start()
    {
        DisplayHintFruit();
    }

    private void OnBeginDrag(Vector2 mousePos)
    {
        cloud.transform.position = new Vector2(mousePos.x , fruitSpawnPoint.position.y);
        if (canSpawnFruit) cloud.ShowLine();

    }
    private void OnDrag(Vector2 mousePos)
    {
        cloud.transform.position = new Vector2(mousePos.x, fruitSpawnPoint.position.y);
        if(canSpawnFruit) cloud.ShowLine();
    }
    private void OnEndDrag(Vector2 mousePos)
    {
        if (canSpawnFruit == false) return;
        if (Time.time - lastSpawnTime < delaySpawnTime) return;
        lastSpawnTime = Time.time;
        canSpawnFruit = false;
        Vector2 spawnPos = new Vector2(mousePos.x, fruitSpawnPoint.position.y);
        spawnner.SpawnCurrentFruit(spawnPos);
        //SoundManager.Instance.PlaySound(SoundName.SpawnFruit);
        cloud.HideLine();
        cloud.HideHintFruit();
    }
    private void DisplayHintFruit()
    {
        cloud.SetNextFruitSprite(spawnner.GetNextFruitSprite());
        cloud.SetCurrentFruitSprite(spawnner.GetCurrentFruitSprite());
    }

    public void OnFruitFirstCol()
    {
        canSpawnFruit = true;
        if(!isUsingSkill)
        {
            DisplayHintFruit();
        }

    }
    public void OnSkillFirstCol()
    {
        canSpawnFruit = true;
        isUsingSkill = false;
        DisplayHintFruit();
    }
    public void UseBoomFruit()
    {
        isUsingSkill = true;
        spawnner.UseBoomFruit();
        cloud.SetCurrentFruitSprite(spawnner.GetCurrentSkillSprite());
    }
    public void UseSwordSkill()
    {
        isUsingSkill = true;
        spawnner.UseSwordSkill();
        cloud.SetCurrentFruitSprite(spawnner.GetCurrentSkillSprite());
    }

    public void DestroyAllFruit()
    {
        StartCoroutine(IE_DestroyFruit());
    }

    private IEnumerator IE_DestroyFruit()
    {
        List<Fruit> listFruits = FindObjectsOfType<Fruit>().ToList();
        yield return new WaitForSeconds(delayStartDestroyAllFruit);
        for(int i = 0; i < listFruits.Count; i++)
        {
            listFruits[i].HideFruit();
            int score = listFruits[i].scoreToAdd;
            effectController.PlayTextPopup(score.ToString(), listFruits[i].transform.position);
            SoundManager.Instance?.PlaySound(SoundName.MergeFruit);
            yield return new WaitForSeconds(delayDestroyAllFruits);
        }
        yield return new WaitForSeconds(1f);
        GameManager.Instance.uiController.ShowGameOverPanel();
    }
}
