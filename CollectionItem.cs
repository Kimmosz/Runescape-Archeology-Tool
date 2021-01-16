using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archeology_Tool {
    class CollectionItem {
        [JsonProperty] private string Name;
        [JsonProperty] private int DamagedAmount;
        [JsonProperty] private int RepairedAmount;

        public CollectionItem(string name, int damagedAmount, int repairedAmount) {
            Name = name;
            DamagedAmount = damagedAmount;
            RepairedAmount = repairedAmount;
        }

        public string GetName() {
            return Name;
        }

        public string GetDamagedName() {
            return Name + " (damaged)";
        }

        public int GetDamagedAmount() {
            return DamagedAmount;
        }

        public int GetRepairedAmount() {
            return RepairedAmount;
        }

        public void AddDamagedArtefact() {
            DamagedAmount++;   
        }
        
        public void RepairDamagedArtefact() {
            DamagedAmount--;
            RepairedAmount++;
        }

        public void HandInArtefact() {
            RepairedAmount--;
        }
    }
}
