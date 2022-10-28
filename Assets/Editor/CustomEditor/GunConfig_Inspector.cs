using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(GunConfig))]
    public class GunConfig_Inspector : Editor
    {
        private SerializedProperty GunName;
        private SerializedProperty DamagePoint;
        private SerializedProperty MaxMagzineAmmo;
        private SerializedProperty MaxBackupAmmo;
        private SerializedProperty AmmoUsePerFire;
        private SerializedProperty BulletType;
        private SerializedProperty BulletProjectileName;
        private SerializedProperty BulletMaxDistance;
        private SerializedProperty FireRoundsPerMinute;
        private SerializedProperty ReloadSpeedMultiple;
        private SerializedProperty IdleAnim;
        private SerializedProperty FireAnim;
        private SerializedProperty ReloadAnim;
        private SerializedProperty BoltAnim;
        private SerializedProperty FireMode;
        private SerializedProperty NeedBolt;
        private SerializedProperty BoltTime;

        private void OnEnable()
        {
            Init();
        }

        public override void OnInspectorGUI()
        {
            DrawCustomGUI();
        }

        private void Init()
        {
            GunName = serializedObject.FindProperty(nameof(GunName));
            DamagePoint = serializedObject.FindProperty(nameof(DamagePoint));
            FireMode = serializedObject.FindProperty(nameof(FireMode));
            NeedBolt = serializedObject.FindProperty(nameof(NeedBolt));
            BoltTime = serializedObject.FindProperty(nameof(BoltTime));
            MaxMagzineAmmo = serializedObject.FindProperty(nameof(MaxMagzineAmmo));
            MaxBackupAmmo = serializedObject.FindProperty(nameof(MaxBackupAmmo));
            AmmoUsePerFire = serializedObject.FindProperty(nameof(AmmoUsePerFire));
            BulletType = serializedObject.FindProperty(nameof(BulletType));
            BulletProjectileName = serializedObject.FindProperty(nameof(BulletProjectileName));
            BulletMaxDistance = serializedObject.FindProperty(nameof(BulletMaxDistance));
            FireRoundsPerMinute = serializedObject.FindProperty(nameof(FireRoundsPerMinute));
            ReloadSpeedMultiple = serializedObject.FindProperty(nameof(ReloadSpeedMultiple));
            IdleAnim = serializedObject.FindProperty(nameof(IdleAnim));
            FireAnim = serializedObject.FindProperty(nameof(FireAnim));
            ReloadAnim = serializedObject.FindProperty(nameof(ReloadAnim));
            BoltAnim = serializedObject.FindProperty(nameof(BoltAnim));

        }

        private void DrawCustomGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(GunName, new GUIContent("Gun Name"));
            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(DamagePoint, new GUIContent("Damage Point"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(FireRoundsPerMinute, new GUIContent("Fire Rounds Per Minute"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(ReloadSpeedMultiple, new GUIContent("Reload Speed Multiple"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(FireMode, new GUIContent("Fire Mode"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(NeedBolt, new GUIContent("Need Bolt"));
            if (NeedBolt.boolValue)
            {
                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(BoltTime, new GUIContent("Bolt Time"));
            }
            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(MaxMagzineAmmo, new GUIContent("Max Magzine Ammo"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(MaxBackupAmmo, new GUIContent("Max Backup Ammo"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(AmmoUsePerFire, new GUIContent("Ammo Use Per Fire"));
            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(BulletType, new GUIContent("Bullet Type"));
            if (BulletType.enumValueIndex == (int)GunConfig.BulletTypeEnum.Projectile)
            {
                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(BulletProjectileName, new GUIContent("Bullet Projectile Name"));
            }
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(BulletMaxDistance, new GUIContent("Bullet Max Distance"));
            EditorGUILayout.Space(10);

            EditorGUILayout.PropertyField(IdleAnim, new GUIContent("Idle Anim"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(FireAnim, new GUIContent("Fire Anim"));
            EditorGUILayout.Space(2);
            EditorGUILayout.PropertyField(ReloadAnim, new GUIContent("Reload Anim"));
            if (NeedBolt.boolValue)
            {
                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(BoltAnim, new GUIContent("Bolt Anim"));
            }
            EditorGUILayout.Space(10);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
