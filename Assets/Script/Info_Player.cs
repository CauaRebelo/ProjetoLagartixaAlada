using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Info_Player
{
    private static int p_health = 3;
    private static bool p_iframe = false;
    private static int p_enchantment = 0;
    private static int p_extra = 0;
    private static int p_deaths = 0;
    private static int p_jumps = 0;
    private static int p_death_fall = 0;
    private static int p_death_enemy1 = 0;
    private static int p_death_enemy2 = 0;
    private static int p_death_projectile1 = 0;
    private static int p_death_projectile2 = 0;

    public static int health
    {
        get { return p_health; }
        set { p_health = value; }
    }

    public static bool iframe
    {
        get { return p_iframe; }
        set { p_iframe = value; }
    }

    public static int enchantment
    {
        get { return p_enchantment; }
        set { p_enchantment = value; }
    }

    public static int extra
    {
        get { return p_extra; }
        set { p_extra = value; }
    }

    public static int deaths
    {
        get { return p_deaths; }
        set { p_deaths = value; }
    }

    public static int jumps
    {
        get { return p_jumps; }
        set { p_jumps = value; }
    }

    public static int death_fall
    {
        get { return p_death_fall; }
        set { p_death_fall = value; }
    }

    public static int death_enemy1
    {
        get { return p_death_enemy1; }
        set { p_death_enemy1 = value; }
    }

    public static int death_enemy2
    {
        get { return p_death_enemy2; }
        set { p_death_enemy2 = value; }
    }

    public static int death_projectile1
    {
        get { return p_death_projectile1; }
        set { p_death_projectile1 = value; }
    }

    public static int death_projectile2
    {
        get { return p_death_projectile2; }
        set { p_death_projectile2 = value; }
    }
}
