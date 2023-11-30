using System;
using TMPro;
using UnityEngine;

public abstract class EnemyCombat : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected TextMeshProUGUI _attackIntentionText;
    [SerializeField] protected TextMeshProUGUI _attackDescriptionText;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _turnTimeAmount = 2f;
    [SerializeField] protected GameObject _iconGameObject;
    
    // [Header("To Attach")]
    // [SerializeField] private AnimatorOverrideController _overrideController;
    

    protected Animator _animator;
    protected CombatManager _combatManager;
    protected PlayerHealth playerHealth;
    private float _turnTimer = 0;
    private bool _isThisEnemyTurn = false;

    private void OnDestroy()
    {
        CombatManager.OnPlayerTurnStart -= PrepareAttack;
    }

    protected virtual void Start()
    {
        
        _animator = GetComponent<Animator>();
        // _animator.runtimeAnimatorController = _overrideController;
        CombatManager.OnPlayerTurnStart += PrepareAttack;
        playerHealth = (PlayerHealth)FindObjectOfType(typeof(PlayerHealth));
        _combatManager = (CombatManager)FindObjectOfType(typeof(CombatManager));
        //_playerEffects = ...
        PrepareAttack();
    }
    
    public virtual void PrepareAttack(){}

    private void Update()
    {
        if (!_isThisEnemyTurn) return;

        _turnTimer += Time.deltaTime;
        if (_turnTimer >= _turnTimeAmount)
        {
            EndTurn();
        }

    }

    private void EndTurn()
    {
        _isThisEnemyTurn = false;
        _turnTimer = 0;
        _combatManager.NextEnemyTurn();
    }

    public virtual void HandleTurn()
    {
        _isThisEnemyTurn = true;
    }

    private void AttackAnimationEvent()
    {
        playerHealth.TakeDamage(_damage);
    }
}