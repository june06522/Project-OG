using System;
using System.Collections.Generic;
using UnityEngine;

// 무기 타입
[Flags]
public enum WeaponType
{
    Gun = 1 << 0,       // 총기류
    Bow = 1 << 1,       // 활
    Throw = 1 << 2,     // 투척류
    Sword = 1 << 3,     // 검
    Spear = 1 << 4,     // 창
    Sickle = 1 << 5,    // 낫
    Blunt = 1 << 6,     // 둔기류
    Whip = 1 << 7,      // 채찍류
}

// 무기별 ID
public enum WeaponID
{
    None,       // 아무것도 아님
    Shotgun,    // 샷건
    Pistol,     // 피스톨
    Crossbow,   // 석궁
    Bow,        // 활
    Slongshot,  // 새총
    Stone,      // 짱돌
    Kunai,      // 쿠나이
    Dagger,     // 단검
    Monkey,     // 몽키스패너
    WaterBang,  // 물풍선
    Katana,     // 카타나
    Branch,     // 나뭇가지
    Spear,      // 창
    Scratcher,  // 효자손
    Golfclub,   // 골프채
    Sickle,     // 낫
    Hammer,     // 망치
    Pickaxe,    // 곡괭이
    Bat,        // 빠따
    Brick,      // 벽돌
    Tennis,     // 테니스라켓
    Rope,       // 로프
    Chain,      // 쇠사슬
}

// 생성기별 ID
public enum GeneratorID
{

}

// Skill 2차원 리스트는 인스펙터에서 안보임 이렇게 해야함
[Serializable]
public class Shell
{
    public List<Skill> skillList;
}

public class SkillManager : MonoBehaviour
{

    private static SkillManager instance;
    public static SkillManager Instance => instance;

    [SerializeField] List<Shell> weaponList;


    public Transform player;

    private void Awake()
    {

        if (instance != null)
        {

            Debug.LogError("Multiple SkillManager is running");
            Destroy(this);

        }

        instance = this;

    }

    /// <summary>
    /// ID에 맞는 스킬 뱉는 애
    /// </summary>
    /// <param name="i">무기 ID</param>
    /// <param name="j">생성기 ID</param>
    /// <returns></returns>
    public Skill GetSKill(int i, int j)
    {

        Debug.Log(2);
        if (weaponList.Count > i && weaponList[i].skillList.Count > j)
        {

            if (weaponList[i] != null && weaponList[j] != null)
            {

                return weaponList[i].skillList[j];

            }

        }

        Debug.LogError($"Skill Doesn't exist in SkillList : {i}, {j} ");
        return null;

    }
}