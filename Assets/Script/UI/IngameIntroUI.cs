using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//인트로 기능 처리
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

    //다른 플레이어 캐릭터 
    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    //글자색
    [SerializeField]
    private Color crewColor;

    [SerializeField]
    private Color imposterColor;


    //코루틴으로 인트로 시퀀스와 임포스터 선택 여부를 순서대로 보여주도록 함
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

        //자신이 임포스터 인 경우 임포스터 UI만 보이게 만들어주고
        // 크루원 인 경우 모든 플레이어가 UI에 보이게 만들어줌

        myCharacter.SetIntrocharacter(myPlayer.nickname, myPlayer.playerColor);

        if(myPlayer.playerType == EPlayerType.Imposter)
        {
            playerType.text = "임포스터";
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
            playerType.text = "크루원";
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
