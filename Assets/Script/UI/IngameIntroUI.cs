using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��Ʈ�� ��� ó��
public class IngameIntroUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shhhhObj;


    [SerializeField]
    private GameObject crewmateObj;

    [SerializeField]
    private Text playerType;

    [SerializeField]
    private Image gradientImg;

    [SerializeField]
    private IntroCharacter myCharacter;

    //�ٸ� �÷��̾� ĳ���� 
    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    //���ڻ�
    [SerializeField]
    private Color crewColor;

    [SerializeField]
    private Color imposterColor;


    //�ڷ�ƾ���� ��Ʈ�� �������� �������� ���� ���θ� ������� �����ֵ��� ��
    public IEnumerator ShowIntroSequence()
    {
        shhhhObj.SetActive(true);
        yield return new WaitForSeconds(3f);

        ShowPlayerType();
        crewmateObj.SetActive(true);
    }


    public void ShowPlayerType()
    {
        var players = GameSystem.Instance.GetPlayerList();

        IngameCharacterMover myPlayer = null;

        foreach(var player in players)
        {
            if (player.hasAuthority)
            {
                myPlayer = player;
                break;
            }
        }

        //�ڽ��� �������� �� ��� �������� UI�� ���̰� ������ְ�
        // ũ��� �� ��� ��� �÷��̾ UI�� ���̰� �������

        myCharacter.SetIntrocharacter(myPlayer.nickname, myPlayer.playerColor);

        if(myPlayer.playerType == EPlayerType.Imposter)
        {
            playerType.text = "��������";
            playerType.color = gradientImg.color = imposterColor;

            int i = 0;
            foreach(var player in players)
            {
                if(!player.hasAuthority && player.playerType == EPlayerType.Imposter)
                {
                    otherCharacters[i].SetIntrocharacter(player.nickname, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        else
        {
            playerType.text = "ũ���";
            playerType.color = gradientImg.color = crewColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.hasAuthority)
                {
                    otherCharacters[i].SetIntrocharacter(player.nickname, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }
}
