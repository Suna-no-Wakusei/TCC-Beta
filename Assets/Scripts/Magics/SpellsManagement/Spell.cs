using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell
{
    public enum SpellType
    {
        Fireball,
        FireSword,
        FlameThrower,
        IceShard,
        LineFreezing,
        AreaFreezing,
        Waterball,
        Squirt,
        WavesControl,
        Speed,
        PushAway,
        Tornado,
        StoneCannon,
        SpikeArmor,
        GuidedCannon,
        Zap,
        Lightning,
        BounceLightning,
    }

    public enum Elements
    {
        Fire,
        Ice,
        Water,
        Air,
        Earth,
        Electricity,
    }


    public SpellType spellType;

    public Sprite GetSprite()
    {
        switch (spellType)
        {
            default:
            case SpellType.Fireball: return SpellAssets.Instance.fireballSprite;
            case SpellType.FireSword: return SpellAssets.Instance.fireSwordSprite;
            case SpellType.FlameThrower: return SpellAssets.Instance.flameThrowerSprite;
            case SpellType.IceShard: return SpellAssets.Instance.iceShardSprite;
            case SpellType.LineFreezing: return SpellAssets.Instance.lineFreezingSprite;
            case SpellType.AreaFreezing: return SpellAssets.Instance.areaFreezingSprite;
            case SpellType.Waterball: return SpellAssets.Instance.waterballSprite;
            case SpellType.Squirt: return SpellAssets.Instance.squirtSprite;
            case SpellType.WavesControl: return SpellAssets.Instance.wavesControlSprite;
            case SpellType.Speed: return SpellAssets.Instance.speedSprite;
            case SpellType.PushAway: return SpellAssets.Instance.pushAwaySprite;
            case SpellType.Tornado: return SpellAssets.Instance.tornadoSprite;
            case SpellType.StoneCannon: return SpellAssets.Instance.stoneCannonSprite;
            case SpellType.SpikeArmor: return SpellAssets.Instance.spikeArmorSprite;
            case SpellType.GuidedCannon: return SpellAssets.Instance.guidedCannonSprite;
            case SpellType.Zap: return SpellAssets.Instance.zapSprite;
            case SpellType.Lightning: return SpellAssets.Instance.lightningSprite;
            case SpellType.BounceLightning: return SpellAssets.Instance.bounceLightningSprite;
        }
    }

    public string SpellName()
    {
        switch (spellType)
        {
            default:
            case SpellType.Fireball: return "Bola de Fogo";
            case SpellType.FireSword: return "Arma Flamejante";
            case SpellType.FlameThrower: return "Lança-Chamas";
            case SpellType.IceShard: return "Cristal de Gelo";
            case SpellType.LineFreezing: return "Congelamento em Linha";
            case SpellType.AreaFreezing: return "Congelamento em Área";
            case SpellType.Waterball: return "Bola d'água";
            case SpellType.Squirt: return "Esguicho";
            case SpellType.WavesControl: return "Controle de Ondas";
            case SpellType.Speed: return "Aprimoramento de Velocidade";
            case SpellType.PushAway: return "Onda de Vento";
            case SpellType.Tornado: return "Tornado";
            case SpellType.StoneCannon: return "Canhão de Pedra";
            case SpellType.SpikeArmor: return "Proteção Espinhosa";
            case SpellType.GuidedCannon: return "Canhão Teleguiado";
            case SpellType.Zap: return "Rajada Elétrica";
            case SpellType.Lightning: return "Relâmpago";
            case SpellType.BounceLightning: return "Raio Elástico";
        }
    }

    public string SpellDesc()
    {
        switch (spellType)
        {
            default:
            case SpellType.Fireball: return "Lança uma bola de fogo que causa 2 de dano mágico";
            case SpellType.FireSword: return "Deixa sua arma atual flamejante, causando 5 de dano mágico adicional ao atacar";
            case SpellType.FlameThrower: return "Incendeia tudo em uma pequena área de cone na direção selecionada, durante o tempo em que mantiver a habilidade ativa, causando 10 de dano mágico";
            case SpellType.IceShard: return "Lança um cristal de gelo que causa 2 de dano mágico";
            case SpellType.LineFreezing: return "Congela tudo na área ao redor de uma linha curta, na direção selecionada, causando 3 de dano mágico";
            case SpellType.AreaFreezing: return "Congela tudo em uma área circular ao redor do personagem, causando 6 de dano mágico";
            case SpellType.Waterball: return "Lança uma bola d'água que causa 2 de dano mágico";
            case SpellType.Squirt: return "Lança uma corrente de água ininterrupta durante o tempo em que mantiver a habilidade ativa, causando 5 de dano mágico";
            case SpellType.WavesControl: return "Lança 3 ondas de água na área ao redor do personagem, causando 7 de dano mágico por onda";
            case SpellType.Speed: return "Aprimora a velocidade base do personagem";
            case SpellType.PushAway: return "Lança uma onda de vento ao redor do personagem, afastando todos ao redor e causando 4 de dano mágico";
            case SpellType.Tornado: return "Invoca um tornado que causa 20 de dano mágico por onde passa";
            case SpellType.StoneCannon: return "Lança um canhão de pedra que causa 2 de dano mágico";
            case SpellType.SpikeArmor: return "Cria uma proteção ao redor do personagem, fazendo com que sofra 5 de dano a menos dos inimigos, e devolva 6 de dano mágico";
            case SpellType.GuidedCannon: return "Lança 5 canhões de pedra que seguem até o inimigo mais próximo, cada um causando 4 de dano mágico";
            case SpellType.Zap: return "Cria uma rajada de eletricidade até o ponto escolhido, causando 1 de dano mágico em todos os inimigos que encostar";
            case SpellType.Lightning: return "Invoca um relâmpago no local selecionado que causa 10 de dano mágico aos inimigos na área";
            case SpellType.BounceLightning: return "Cria uma rajada de eletricidade que bate e ricocheteia em até 6 inimigos, causando 15 de dano mágico em cada um";
        }
    }

    public int SpellRank()
    {
        switch (spellType)
        {
            default:
            case SpellType.Fireball: return 1;
            case SpellType.FireSword: return 2;
            case SpellType.FlameThrower: return 3;
            case SpellType.IceShard: return 1;
            case SpellType.LineFreezing: return 2;
            case SpellType.AreaFreezing: return 3;
            case SpellType.Waterball: return 1;
            case SpellType.Squirt: return 2;
            case SpellType.WavesControl: return 3;
            case SpellType.Speed: return 1;
            case SpellType.PushAway: return 2;
            case SpellType.Tornado: return 3;
            case SpellType.StoneCannon: return 1;
            case SpellType.SpikeArmor: return 2;
            case SpellType.GuidedCannon: return 3;
            case SpellType.Zap: return 1;
            case SpellType.Lightning: return 2;
            case SpellType.BounceLightning: return 3;
        }
    }

    public int SpellCost()
    {
        switch (spellType)
        {
            default:
            case SpellType.Fireball: return 1;
            case SpellType.FireSword: return 3;
            case SpellType.FlameThrower: return 2;
            case SpellType.IceShard: return 1;
            case SpellType.LineFreezing: return 3;
            case SpellType.AreaFreezing: return 5;
            case SpellType.Waterball: return 1;
            case SpellType.Squirt: return 1;
            case SpellType.WavesControl: return 3;
            case SpellType.Speed: return 1;
            case SpellType.PushAway: return 2;
            case SpellType.Tornado: return 4;
            case SpellType.StoneCannon: return 1;
            case SpellType.SpikeArmor: return 3;
            case SpellType.GuidedCannon: return 3;
            case SpellType.Zap: return 1;
            case SpellType.Lightning: return 3;
            case SpellType.BounceLightning: return 3;
        }
    }

    public Elements SpellElement()
    {
        switch (spellType)
        {
            default:
            case SpellType.Fireball: return Elements.Fire;
            case SpellType.FireSword: return Elements.Fire;
            case SpellType.FlameThrower: return Elements.Fire;
            case SpellType.IceShard: return Elements.Ice;
            case SpellType.LineFreezing: return Elements.Ice;
            case SpellType.AreaFreezing: return Elements.Ice;
            case SpellType.Waterball: return Elements.Water;
            case SpellType.Squirt: return Elements.Water;
            case SpellType.WavesControl: return Elements.Water;
            case SpellType.Speed: return Elements.Air;
            case SpellType.PushAway: return Elements.Air;
            case SpellType.Tornado: return Elements.Air;
            case SpellType.StoneCannon: return Elements.Earth;
            case SpellType.SpikeArmor: return Elements.Earth;
            case SpellType.GuidedCannon: return Elements.Earth;
            case SpellType.Zap: return Elements.Electricity;
            case SpellType.Lightning: return Elements.Electricity;
            case SpellType.BounceLightning: return Elements.Electricity;
        }
    }

    
}
