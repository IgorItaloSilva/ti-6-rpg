using System;

public class RuneEvents{
    public event Action<bool,int> onRuneDamageBuff;
    
    public void RuneDamageBuff(bool isApply,int value){
        if(onRuneDamageBuff!=null){
            onRuneDamageBuff(isApply,value);
        }
    }
}
