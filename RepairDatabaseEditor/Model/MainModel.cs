﻿using Reactive.Bindings;
using RepairDatabaseEditor.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairDatabaseEditor.Model
{
    class MainModel
    {
        /// <summary>
        /// データベース
        /// </summary>
        private DataStore dataStore;

        /// <summary>
        /// 艦娘一覧
        /// </summary>
        public ObservableCollection<Kammusu> KammusuList { get; }

        /// <summary>
        /// 選択中の艦娘
        /// </summary>
        public ReactiveProperty<Kammusu> SelectedKammusu { get; set; } = new ReactiveProperty<Kammusu>(new Kammusu());

        /// <summary>
        /// 艦娘の番号
        /// </summary>
        public ReactiveProperty<string> KammusuId { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 艦娘の名前
        /// </summary>
        public ReactiveProperty<string> KammusuName { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 装備一覧
        /// </summary>
        public ObservableCollection<Weapon> WeaponList { get; }

        /// <summary>
        /// 選択中の装備
        /// </summary>
        public ReactiveProperty<Weapon> SelectedWeapon { get; set; } = new ReactiveProperty<Weapon>(new Weapon());

        /// <summary>
        /// 装備の番号
        /// </summary>
        public ReactiveProperty<string> WeaponId { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// 装備の名前
        /// </summary>
        public ReactiveProperty<string> WeaponName { get; } = new ReactiveProperty<string>("");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataStore">DIするデータベース</param>
        public MainModel(DataStore dataStore)
        {
            // リストボックスに標示するデータを読み込み
            this.dataStore = dataStore;
            KammusuList = new ObservableCollection<Kammusu>(dataStore.Kammusus);
            WeaponList = new ObservableCollection<Weapon>(dataStore.Weapons);

            // 選択変更時の処理を記述
            SelectedKammusu.Subscribe(value => {
                KammusuId.Value = value.Id.ToString();
                KammusuName.Value = value.Name;
            });
            SelectedWeapon.Subscribe(value => {
                WeaponId.Value = value.Id.ToString();
                WeaponName.Value = value.Name;
            });
        }
    }
}
