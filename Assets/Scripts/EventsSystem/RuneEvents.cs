using System;

public class RuneEvents{
    public event Action<bool,int> onRuneDamageBuff;
    public void RuneDamageBuff(bool isApply,int value){
        if(onRuneDamageBuff!=null){
            onRuneDamageBuff(isApply,value);
        }
    }
    public event Action<bool,string,int> onRuneStatBuff;
    public void RuneStatBuff(bool isApply,string stat,int value){
        if(onRuneStatBuff!=null){
            onRuneStatBuff(isApply,stat,value);
        }
    }
    public event Action<bool,Enums.RuneOtherCode,int> onRuneOtherBuff;
    public void RuneOtherBuff(bool isApply, Enums.RuneOtherCode code,int value){
        if(onRuneOtherBuff!=null){
            onRuneOtherBuff(isApply,code,value);
        }
    }
}
