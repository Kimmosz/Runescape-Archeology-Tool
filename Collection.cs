using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archeology_Tool {
    class Collection {
        [JsonProperty] private string Name;
        [JsonProperty] private int Level;
        [JsonProperty] private CollectionItem[] Artefacts;

        public Collection(string name, int level, CollectionItem[] artefacts) {
            Name = name;
            Level = level;
            Artefacts = artefacts;
        }

        public string GetCollectionName() {
            return Name;
        }

        public int GetCollectionLevel() {
            return Level;
        }

        public CollectionItem GetArtefact(int artefactPosition) {
            return Artefacts[artefactPosition];
        }

        public CollectionItem[] GetAllArtefacts() {
            return Artefacts;
        }

        public void HandInCollection() {
            foreach (CollectionItem artefact in Artefacts) {
                artefact.HandInArtefact();
            }
        }
    }
}
