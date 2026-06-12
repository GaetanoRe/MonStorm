using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CharacterAI : MonoBehaviour
{
	public bool isHero = false; //these bools are only used to play the correct animations right now, but may be useful in other ways later
	public bool isDragon = false;
	public bool isStag = false;

    [Header("GameObjects")]
	public GameObject targetObject; //what this character is focused on NPC/enemy/player
    public GameObject currentWeapon;
    public GameObject currentArmor;

    [Header("State Bools")] //general state bools to keep track of what character is doing
    private bool stateChanged = false; //!!always turn this false before changing main states
	private bool idleGO = false;
	private bool walkGO = false;
	private bool runGO = false;
    private bool sneakGO = false;
	private bool attackGO = false;
	private bool jumpGO = false;
	private bool defendGO = false;
	private bool damagedGO = false;
    private bool fleeGO = false;
    private bool defeatedGO = false;

    [Header("Other Bools")]
    public bool inBattle = false;
	private bool damageTaken = false;
	private bool idleAnimationPlayed = false;
	private bool walkAnimationPlayed = false;
	private bool runAnimationPlayed = false;
	private bool sneakAnimationPlayed = false;
	private bool attackAnimationPlayed = false;
	private bool jumpAnimationPlayed = false;
	private bool defendAnimationPlayed = false;
	private bool damagedAnimationPlayed = false;
	private bool fleeAnimationPlayed = false;
	private bool defeatedAnimationPlayed = false;
    private bool attackAudioPlayed = false;
    private bool defeatedAudioPlayed = false;
    private bool okayToPlayAudio = false;
    private bool attackAudioFirstPlayed = false;
    private bool attackAudioSecondPlayed = false;
    private bool defeatedAudioFirstPlayed = false;
    private bool defeatedAudioSecondPlayed = false;

    [Header("General Floats.")]
	public float targetDistance; //distance to target
    public float warningDistance = 12f; //distance to danger
	public float safeDistance = 16;
    public float moveSpeed = 3; // The nav mesh agent's speed when walking
	public float pursueSpeed = 5; // The nav mesh agent's speed when pursuing


    [Header("Audio Floats.")] //these help with random dialog lines, to avoid repetition
    public float lastPlayedAttackAudio;
	public float lastPlayedAttackSecondAudio;
    public float lastPlayedDefeatedAudio;
	public float lastPlayedDefeatedSecondAudio;


    [Header("Script References.")]
    public GameObject gameController;
	//private GameController m_GC; //to be linked later
    private Animator anim;
    private Rigidbody rb;
	private AudioSource audioSource;
	private UnityEngine.AI.NavMeshAgent nav;

    [Header("Character Audio Clips.")]
    public AudioClip [] generalAudio;
	private AudioClip generalClip;
	public AudioClip [] attackAudio;
	private AudioClip attackClip;
    public AudioClip [] defeatedAudio;
	private AudioClip defeatedClip;

	WaitForSeconds pointFiveThree = new WaitForSeconds(0.53f); //it's more efficient to store WaitForSeconds for coroutines
	WaitForSeconds pointSixSixSeven = new WaitForSeconds(0.667f);
	WaitForSeconds one = new WaitForSeconds(1f);
	WaitForSeconds onePointTwoSix = new WaitForSeconds(1.26f);
    WaitForSeconds onePointNine = new WaitForSeconds(1.9f);
    WaitForSeconds two = new WaitForSeconds(2f);

    //State Machine References:--------------------
    public enum ActionType //main states
	{
		PreIdle,
		Idle,
		Walk,
        Run,
		Sneak,
		PreAttack,
		Attack,
		PreJump,
		Jump,
		Defend,
		Damaged,
		PreFlee,
		Flee,
		Defeated
	}

    private ActionType eCurState = ActionType.Idle;
    //-----------------------------------------------------
	public enum SubActionTypeIdle //sub action state for more nuance within the larger states
	{
		StandardIdle,
		BattleIdle
	}

	private SubActionTypeIdle eCurSubStateIdle = SubActionTypeIdle.StandardIdle;
    //-----------------------------------------------------
	public enum SubActionTypeWalk //sub action state
	{
		StandardWalk,
		BattleWalk
	}

	private SubActionTypeWalk eCurSubStateWalk = SubActionTypeWalk.StandardWalk;
    //-----------------------------------------------------
	public enum SubActionTypeSneak //sub action state
	{
		StandardSneak,
		BattleSneak
	}
    private SubActionTypeSneak eCurSubStateSneak = SubActionTypeSneak.StandardSneak;
    //-----------------------------------------------------
	public enum SubActionTypeRun //sub action state
	{
		StandardRun,
		BattleRun
	}

	private SubActionTypeRun eCurSubStateRun = SubActionTypeRun.StandardRun;
    //-----------------------------------------------------
	public enum SubActionTypeAttack //sub action state
	{
		StandardAttack,
		BattleAttack
	}

	private SubActionTypeAttack eCurSubStateAttack = SubActionTypeAttack.StandardAttack;
	//-----------------------------------------------------
	public enum SubActionTypeJump //sub action state
	{
		StandardJump,
		BattleJump
	}

	private SubActionTypeJump eCurSubStateJump = SubActionTypeJump.StandardJump;
    //-----------------------------------------------------
	public enum SubActionTypeDefend //sub action state
	{
		StandardDefend,
		BattleDefend
	}

	private SubActionTypeDefend eCurSubStateDefend = SubActionTypeDefend.StandardDefend;
	//-----------------------------------------------------
	public enum SubActionTypeDamaged //sub action state
	{
		StandardDamaged,
		BattleDamaged
	}

	private SubActionTypeDamaged eCurSubStateDamaged = SubActionTypeDamaged.StandardDamaged;
    //-----------------------------------------------------
	public enum SubActionTypeFlee //sub action state
	{
		StandardFlee,
		BattleFlee
	}

	private SubActionTypeFlee eCurSubStateFlee = SubActionTypeFlee.StandardFlee;
    //-----------------------------------------------------
	public enum SubActionTypeDefeated //sub action state
	{
		StandardDefeated,
		BattleDefeated
	}

	private SubActionTypeDefeated eCurSubStateDefeated = SubActionTypeDefeated.StandardDefeated;

    void Start()
    {
        stateChanged = false;
	    idleGO = true;
	    walkGO = false;
	    runGO = false;
        sneakGO = false;
	    attackGO = false;
		jumpGO = false;
	    defendGO = false;
		damagedGO = false;
        fleeGO = false;
        defeatedGO = false;
        inBattle = false;
		damageTaken = false;
		idleAnimationPlayed = false;
		walkAnimationPlayed = false;
		runAnimationPlayed = false;
		sneakAnimationPlayed = false;
		attackAnimationPlayed = false;
		jumpAnimationPlayed = false;
		defendAnimationPlayed = false;
		damagedAnimationPlayed = false;
		fleeAnimationPlayed = false;
		defeatedAnimationPlayed = false;
        attackAudioPlayed = false;
        defeatedAudioPlayed = false;
        okayToPlayAudio = false;
        attackAudioFirstPlayed = false;
        attackAudioSecondPlayed = false;
        defeatedAudioFirstPlayed = false;
        defeatedAudioSecondPlayed = false;

        //m_GC = gameController.GetComponent<GameController> (); //to be uncommented out once GC in place
        anim = GetComponent<Animator>();
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource> ();

		HandleIdleState ();
    }

    void Update()
    {
        switch (eCurState)
		{
		case ActionType.PreIdle:
			HandlePreIdleState();
			break;

		case ActionType.Idle:
			HandleIdleState();
			break;

		case ActionType.Walk:
			HandleWalkState();
			break;

		case ActionType.Run:
			HandleRunState();
			break;

		case ActionType.Sneak:
			HandleSneakState();
			break;

		case ActionType.PreAttack:
			HandlePreAttackState();
			break;

		case ActionType.Attack:
			HandleAttackState();
			break;

		case ActionType.PreJump:
			HandlePreJumpState();
			break;

		case ActionType.Jump:
			HandleJumpState();
			break;

		case ActionType.Defend:
			HandleDefendState();
			break;

		case ActionType.Damaged:
			HandleDamagedState();
			break;

		case ActionType.PreFlee:
			HandlePreFleeState();
			break;

		case ActionType.Flee:
			HandleFleeState();
			break;

		case ActionType.Defeated:
			HandleDefeatedState();
			break;
		}

        if (idleGO) {
			switch (eCurSubStateIdle) {
			case SubActionTypeIdle.StandardIdle:
				HandleStandardIdleState ();
				break;

			case SubActionTypeIdle.BattleIdle:
				HandleBattleIdleState ();
				break;
			}
		}
        if (walkGO) {
			switch (eCurSubStateWalk) {
			case SubActionTypeWalk.StandardWalk:
				HandleStandardWalkState ();
				break;

			case SubActionTypeWalk.BattleWalk:
				HandleBattleWalkState ();
				break;
			}
		}
        if (runGO) {
			switch (eCurSubStateRun) {
			case SubActionTypeRun.StandardRun:
				HandleStandardRunState ();
				break;

			case SubActionTypeRun.BattleRun:
				HandleBattleRunState ();
				break;
			}
		}
        if (sneakGO) {
			switch (eCurSubStateSneak) {
			case SubActionTypeSneak.StandardSneak:
				HandleStandardSneakState ();
				break;

			case SubActionTypeSneak.BattleSneak:
				HandleBattleSneakState ();
				break;
			}
		}
        if (attackGO) {
			switch (eCurSubStateAttack) {
			case SubActionTypeAttack.StandardAttack:
				HandleStandardAttackState ();
				break;

			case SubActionTypeAttack.BattleAttack:
				HandleBattleAttackState ();
				break;
			}
		}
		if (jumpGO) {
			switch (eCurSubStateJump) {
			case SubActionTypeJump.StandardJump:
				HandleStandardJumpState ();
				break;

			case SubActionTypeJump.BattleJump:
				HandleBattleJumpState ();
				break;
			}
		}
        if (defendGO) {
			switch (eCurSubStateDefend) {
			case SubActionTypeDefend.StandardDefend:
				HandleStandardDefendState ();
				break;

			case SubActionTypeDefend.BattleDefend:
				HandleBattleDefendState ();
				break;
			}
		}
		if (damagedGO) {
			switch (eCurSubStateDamaged) {
			case SubActionTypeDamaged.StandardDamaged:
				HandleStandardDamagedState ();
				break;

			case SubActionTypeDamaged.BattleDamaged:
				HandleBattleDamagedState ();
				break;
			}
		}
        if (fleeGO) {
			switch (eCurSubStateFlee) {
			case SubActionTypeFlee.StandardFlee:
				HandleStandardFleeState ();
				break;

			case SubActionTypeFlee.BattleFlee:
				HandleBattleFleeState ();
				break;
			}
		}
        if (defeatedGO) {
			switch (eCurSubStateDefeated) {
			case SubActionTypeDefeated.StandardDefeated:
				HandleStandardDefeatedState ();
				break;

			case SubActionTypeDefeated.BattleDefeated:
				HandleBattleDefeatedState ();
				break;
			}
		}

		targetDistance = Vector3.Distance (targetObject.transform.position, transform.position);
		if ((targetDistance <= warningDistance) && (!inBattle)) {
			inBattle = true;
			if (isDragon) {
				stateChanged = false;
				HandleAttackState ();
			} else if (isStag) {
				nav.isStopped = false;
				fleeAnimationPlayed = false;
				stateChanged = false;
				HandleFleeState ();
			}
		}
		if ((targetDistance > safeDistance) && (inBattle)) {
			inBattle = false;
		}
    }

    //IDLE STATE------------------------------------------
    public void HandlePreIdleState ()
	{
		eCurState = ActionType.PreIdle;

		idleAnimationPlayed = false;
		stateChanged = false;
		HandleIdleState ();
	}

	void HandleIdleState ()
	{
		eCurState = ActionType.Idle;

		if (!stateChanged) {
			idleGO = true;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			if (!inBattle) {
				HandleStandardIdleState ();
			} else if (inBattle) {
				HandleBattleIdleState ();
			}

			//idleAnimationPlayed = false;
			walkAnimationPlayed = false;
			runAnimationPlayed = false;
			sneakAnimationPlayed = false;
			attackAnimationPlayed = false;
			jumpAnimationPlayed = false;
			defendAnimationPlayed = false;
			damagedAnimationPlayed = false;
			fleeAnimationPlayed = false;
			defeatedAnimationPlayed = false;

			stateChanged = true;
		}
	}

	void HandleStandardIdleState ()
	{
		eCurSubStateIdle = SubActionTypeIdle.StandardIdle;

		if (!idleAnimationPlayed) {
			if (isHero) {
				anim.Play ("Idle");
			} else if (isStag) {
				anim.Play ("idle");
			}
			idleAnimationPlayed = true;
		}
	}

    void HandleBattleIdleState ()
	{
		eCurSubStateIdle = SubActionTypeIdle.BattleIdle;

		if (!idleAnimationPlayed) {
			if (isHero) {
				anim.Play ("IdleCombat");
			} else if (isStag) {
				anim.Play ("idle");
			}
			idleAnimationPlayed = true;
		}
	}

    //WALK STATE------------------------------------------
    void HandleWalkState ()
	{
		eCurState = ActionType.Walk;

		if (!stateChanged) {
			idleGO = false;
			walkGO = true;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			walkAnimationPlayed = false;

			HandleStandardWalkState ();

			stateChanged = true;
		}
	}

	void HandleStandardWalkState ()
	{
		eCurSubStateWalk = SubActionTypeWalk.StandardWalk;

		if (!walkAnimationPlayed) {
			if (isHero) {
				//anim.Play ("RunForward"); //he doesn't have a walk ani...
			} else if (isDragon) {
				anim.Play ("walking");
			} else if (isStag) {
				anim.Play ("walking");
			}
			walkAnimationPlayed = true;
		}
	}

    void HandleBattleWalkState ()
	{
		eCurSubStateWalk = SubActionTypeWalk.BattleWalk;

		
	}

    //RUN STATE------------------------------------------
	public void HandlePreRunState ()
	{
		stateChanged = false;
		HandleRunState ();
	}

    void HandleRunState ()
	{
		eCurState = ActionType.Run;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = true;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			runAnimationPlayed = false;

			HandleStandardRunState ();

			stateChanged = true;
		}
	}

	void HandleStandardRunState ()
	{
		eCurSubStateRun = SubActionTypeRun.StandardRun;

		if (!runAnimationPlayed) {
			if (isHero) {
				anim.Play ("RunForward");
			} else if (isDragon) {
				anim.Play ("running");
			} else if (isStag) {
				anim.Play ("running");
			}
			runAnimationPlayed = true;
		}
	}

    void HandleBattleRunState ()
	{
		eCurSubStateRun = SubActionTypeRun.BattleRun;

		
	}

    //SNEAK STATE------------------------------------------
    void HandleSneakState ()
	{
		eCurState = ActionType.Sneak;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = true;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			stateChanged = true;
		}
	}

	void HandleStandardSneakState ()
	{
		eCurSubStateSneak = SubActionTypeSneak.StandardSneak;

		
	}

    void HandleBattleSneakState ()
	{
		eCurSubStateSneak = SubActionTypeSneak.BattleSneak;

		
	}

    //ATTACK STATE------------------------------------------
	public void HandlePreAttackState ()
	{	
		eCurState = ActionType.PreAttack;

		attackAnimationPlayed = false;
		stateChanged = false;
		HandleAttackState ();
		stateChanged = false;
	}

    void HandleAttackState ()
	{
		eCurState = ActionType.Attack;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = true;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			if (!inBattle) {
				HandleStandardAttackState ();
			} else if (inBattle) {
				HandleBattleAttackState ();
			}

			stateChanged = true;
		}
	}

	void HandleStandardAttackState ()
	{
		eCurSubStateAttack = SubActionTypeAttack.StandardAttack;

		if (!attackAnimationPlayed) {
			if (isHero) {
				anim.Play ("PunchRight"); //1
				ResetStandardAttack ();
			} else if (isDragon) {
				anim.Play ("attack");
				ResetDragonAttack ();
			}
			attackAnimationPlayed = true;
		}

		if (!attackAudioPlayed) {
		   	okayToPlayAudio = false;
		   	PlayAttackAudio ();
            attackAudioPlayed = true;
        }
	}

    void HandleBattleAttackState ()
	{
		eCurSubStateAttack = SubActionTypeAttack.BattleAttack;

		if (!attackAnimationPlayed) {
			if (isHero) {
				anim.Play ("MeleeAttack_TwoHanded"); //1.9
				ResetBattleAttack ();
			} else if (isDragon) {
				anim.Play ("attack");
				ResetDragonAttack ();
			}
			attackAnimationPlayed = true;
		}

		if (!attackAudioPlayed) {
		   	okayToPlayAudio = false;
		   	PlayAttackAudio ();
            attackAudioPlayed = true;
        }
	}

	//JUMP STATE------------------------------------------
	public void HandlePreJumpState ()
	{
		eCurState = ActionType.PreJump;

		jumpAnimationPlayed = false;
		stateChanged = false;
		HandleJumpState ();
	}

    void HandleJumpState ()
	{
		eCurState = ActionType.Jump;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = true;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			jumpAnimationPlayed = false;

			HandleStandardJumpState ();

			stateChanged = true;
		}
	}

	void HandleStandardJumpState ()
	{
		eCurSubStateJump = SubActionTypeJump.StandardJump;

		if (!jumpAnimationPlayed) {
			if (isHero) {
				anim.Play ("Jump_Up");
				TransitionToIdleFromJump ();
			} else if (isDragon) {
				anim.Play ("takingOff");
			}
			jumpAnimationPlayed = true;
		}
	}

    void HandleBattleJumpState ()
	{
		eCurSubStateJump = SubActionTypeJump.BattleJump;

		
	}

    //DEFEND STATE------------------------------------------
    void HandleDefendState ()
	{
		eCurState = ActionType.Defend;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = true;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = false;

			stateChanged = true;
		}
	}

	void HandleStandardDefendState ()
	{
		eCurSubStateDefend = SubActionTypeDefend.StandardDefend;

		
	}

    void HandleBattleDefendState ()
	{
		eCurSubStateDefend = SubActionTypeDefend.BattleDefend;

		
	}

	//DAMAGED STATE------------------------------------------
    void HandleDamagedState ()
	{
		eCurState = ActionType.Damaged;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = true;
			fleeGO = false;
			defeatedGO = false;

			damagedAnimationPlayed = false;

			HandleStandardDamagedState ();

			stateChanged = true;
		}
	}

	void HandleStandardDamagedState ()
	{
		eCurSubStateDamaged = SubActionTypeDamaged.StandardDamaged;

		if (!damagedAnimationPlayed) {
			if (isHero) {
				anim.Play ("GetHit"); //.667
				TransitionToIdle ();
			} else if (isDragon) {
				anim.Play ("hit");
			} else if (isStag) {
				anim.Play ("hit");
			}
			damagedAnimationPlayed = true;
		}
	}

    void HandleBattleDamagedState ()
	{
		eCurSubStateDamaged = SubActionTypeDamaged.BattleDamaged;

		
	}

    //FLEE STATE------------------------------------------
	public void HandlePreFleeState ()
	{
		eCurState = ActionType.PreFlee;

		fleeAnimationPlayed = false;
		if (isStag) {
			nav.isStopped = false;
		}
		stateChanged = false;
		HandleFleeState ();
	}

    void HandleFleeState ()
	{
		eCurState = ActionType.Flee;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = true;
			defeatedGO = false;

			fleeAnimationPlayed = false;

			if (!inBattle) {
				HandleStandardFleeState ();
			} else if (inBattle) {
				HandleBattleFleeState ();
			}

			stateChanged = true;
		}
	}

	void HandleStandardFleeState ()
	{
		eCurSubStateFlee = SubActionTypeFlee.StandardFlee;

		if (!fleeAnimationPlayed) {
			if (isHero) {
				anim.Play ("RollBackward"); //1.267
				TransitionToIdleFromFlee ();
			} else if (isStag) {
				anim.Play ("running");	
			}	
			fleeAnimationPlayed = true;
		}

		if ((isStag) && (inBattle)) {
			nav.speed = moveSpeed;
			Vector3 runTo = transform.position + ((transform.position - targetObject.transform.position));
			nav.SetDestination (runTo);
		}

		if ((isStag) && (!inBattle)) {
			stateChanged = false;
			HandleIdleState ();
			nav.isStopped = true;
		}
	}

    void HandleBattleFleeState ()
	{
		eCurSubStateFlee = SubActionTypeFlee.BattleFlee;

		if (!fleeAnimationPlayed) {
			if (isHero) {
				anim.Play ("RollBackward"); //1.267
				TransitionToIdleFromFlee ();
			} else if (isStag) {
				anim.Play ("running");	
			}	
			fleeAnimationPlayed = true;
		}

		if (isStag) {
			nav.speed = moveSpeed;
			Vector3 runTo = transform.position + ((transform.position - targetObject.transform.position));
			nav.SetDestination (runTo);
		}

		if ((isStag) && (!inBattle)) {
			stateChanged = false;
			anim.Play ("idle");
			HandleIdleState ();
			nav.isStopped = true;
			fleeAnimationPlayed = false;
		}
	}

    //DEFEATED STATE------------------------------------------
    void HandleDefeatedState ()
	{
		eCurState = ActionType.Defeated;

		if (!stateChanged) {
			idleGO = false;
			walkGO = false;
			runGO = false;
			sneakGO = false;
			attackGO = false;
			jumpGO = false;
			defendGO = false;
			damagedGO = false;
			fleeGO = false;
			defeatedGO = true;

			defeatedAnimationPlayed = false;

			HandleStandardDefeatedState ();

			stateChanged = true;
		}

		if (!defeatedAudioPlayed) {
		    okayToPlayAudio = false;
		    PlayDefeatedAudio ();
            defeatedAudioPlayed = true;
        }
	}

	void HandleStandardDefeatedState ()
	{
		eCurSubStateDefeated = SubActionTypeDefeated.StandardDefeated;

		if (!defeatedAnimationPlayed) {
			if (isHero) {
				anim.Play ("Death");
			} else if (isDragon) {
				anim.Play ("dying");
			} else if (isStag) {
				anim.Play ("dying");
			}
			defeatedAnimationPlayed = true;
		}
	}

    void HandleBattleDefeatedState ()
	{
		eCurSubStateDefeated = SubActionTypeDefeated.BattleDefeated;

		
	}

    //--------------------------------------------------------------------------------

    void PlayAttackAudio ()
	{
		StartCoroutine (CR_AttackAudio ());
	}

	private IEnumerator CR_AttackAudio ()
	{
		yield return null;

		int index = Random.Range (0, 2);
		attackClip = attackAudio [index];
		audioSource.clip = attackClip;

		if ((attackAudioFirstPlayed) && (attackAudioSecondPlayed)) {
			if ((audioSource.clip.length == lastPlayedAttackAudio) || (audioSource.clip.length == lastPlayedAttackSecondAudio)) {
				
				PlayAttackAudio ();
				okayToPlayAudio = false;
			} else if ((audioSource.clip.length != lastPlayedAttackAudio) && (audioSource.clip.length != lastPlayedAttackSecondAudio)) {
				
				audioSource.Play ();
				lastPlayedAttackSecondAudio = lastPlayedAttackAudio;
				lastPlayedAttackAudio = audioSource.clip.length;
				okayToPlayAudio = true;
			}
		}
		else if ((attackAudioFirstPlayed) && (!attackAudioSecondPlayed)) {
			if (audioSource.clip.length == lastPlayedAttackAudio) {
				
				PlayAttackAudio ();
				okayToPlayAudio = false;
			} else if (audioSource.clip.length != lastPlayedAttackAudio) {
				
				audioSource.Play ();
				lastPlayedAttackSecondAudio = lastPlayedAttackAudio;
				lastPlayedAttackAudio = audioSource.clip.length;
				attackAudioSecondPlayed = true;
				okayToPlayAudio = true;
			}
		}
		else if ((!attackAudioFirstPlayed) && (!attackAudioSecondPlayed)) {
			
			audioSource.Play ();
			lastPlayedAttackAudio = audioSource.clip.length;
			attackAudioFirstPlayed = true;
			okayToPlayAudio = true;
		}
	}

    void PlayDefeatedAudio ()
	{
		StartCoroutine (CR_DefeatedAudio ());
	}

	private IEnumerator CR_DefeatedAudio ()
	{
		yield return null;

		int index = Random.Range (0, 3);
		defeatedClip = defeatedAudio [index];
		audioSource.clip = defeatedClip;

		if ((defeatedAudioFirstPlayed) && (defeatedAudioSecondPlayed)) {
			if ((audioSource.clip.length == lastPlayedDefeatedAudio) || (audioSource.clip.length == lastPlayedDefeatedSecondAudio)) {
				
				PlayDefeatedAudio ();
				okayToPlayAudio = false;
			} else if ((audioSource.clip.length != lastPlayedDefeatedAudio) && (audioSource.clip.length != lastPlayedDefeatedSecondAudio)) {
				
				audioSource.Play ();
				lastPlayedDefeatedSecondAudio = lastPlayedDefeatedAudio;
				lastPlayedDefeatedAudio = audioSource.clip.length;
				okayToPlayAudio = true;
			}
		}
		else if ((defeatedAudioFirstPlayed) && (!defeatedAudioSecondPlayed)) {
			if (audioSource.clip.length == lastPlayedDefeatedAudio) {
				
				PlayDefeatedAudio ();
				okayToPlayAudio = false;
			} else if (audioSource.clip.length != lastPlayedDefeatedAudio) {
				
				audioSource.Play ();
				lastPlayedDefeatedSecondAudio = lastPlayedDefeatedAudio;
				lastPlayedDefeatedAudio = audioSource.clip.length;
				defeatedAudioSecondPlayed = true;
				okayToPlayAudio = true;
			}
		}
		else if ((!defeatedAudioFirstPlayed) && (!defeatedAudioSecondPlayed)) {
			
			audioSource.Play ();
			lastPlayedDefeatedAudio = audioSource.clip.length;
			defeatedAudioFirstPlayed = true;
			okayToPlayAudio = true;
		}
	}

	// COROUTINES------------------------------------------------

	void ResetStandardAttack ()
	{
		StartCoroutine (CR_ResetStandardAttack ());
	}

	private IEnumerator CR_ResetStandardAttack ()
	{
		yield return one;

		attackAnimationPlayed = false;
		attackAudioPlayed = false;
	}

	void ResetBattleAttack ()
	{
		StartCoroutine (CR_ResetBattleAttack ());
	}

	private IEnumerator CR_ResetBattleAttack ()
	{
		yield return onePointNine;

		attackAnimationPlayed = false;
		attackAudioPlayed = false;
	}

	void ResetDragonAttack ()
	{
		StartCoroutine (CR_ResetDragonAttack ());
	}

	private IEnumerator CR_ResetDragonAttack ()
	{
		yield return two;

		attackAnimationPlayed = false;
		attackAudioPlayed = false;

		stateChanged = false;
		HandleIdleState ();
	}

	void TransitionToIdle ()
	{
		StartCoroutine (CR_TransitionToIdle ());
	}

	private IEnumerator CR_TransitionToIdle ()
	{
		yield return pointSixSixSeven;

		stateChanged = false;
		HandleIdleState ();
	}

	void TransitionToIdleFromJump ()
	{
		StartCoroutine (CR_TransitionToIdleFromJump ());
	}

	private IEnumerator CR_TransitionToIdleFromJump ()
	{
		yield return pointFiveThree;

		stateChanged = false;
		HandleIdleState ();
	}

	void TransitionToIdleFromFlee ()
	{
		StartCoroutine (CR_TransitionToIdleFromFlee ());
	}

	private IEnumerator CR_TransitionToIdleFromFlee ()
	{
		yield return onePointTwoSix;

		stateChanged = false;
		HandleIdleState ();
	}

	// ON FUNCTIONS------------------------------------------------
	void OnCollisionEnter(Collision col)
	{
		if ((col.gameObject.tag == "weapon") && (!damageTaken)) {
			//if HP != 0
			stateChanged = false;
			HandleDamagedState ();
			//else if HP <= 0
			//HandleDefeatedState ();
			damageTaken = true;
		}
	}

	//void OnTriggerEnter(Collider coll) //probably need this at some point
	//{
		//if (coll.gameObject.tag == "fuzz") {

		//}

	//}
}
