using System;

[Serializable]
public class DAZMorphFormula
//the formula that how current morph affect other morphes with targetMorph=morphValue*multiplier
{
	public DAZMorphFormulaTargetType targetType;

	public string target;

	public float multiplier;
}
