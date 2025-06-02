using System;


namespace Script{
    [Serializable]
    public class AlbumEntry
    {
        public string nome;
        public string rarita;

        // Costruttore
        public AlbumEntry(string nome, string rarita)
        {
            this.nome = nome;
            this.rarita = rarita;
        }

        // faccio un equals per verificare che non esista una carta con lo stesso nome e rarit� nell'album
        public override bool Equals(object obj)
        {
            if (obj is AlbumEntry other)
                return this.nome == other.nome && this.rarita == other.rarita;
            return false;
        }

        public override int GetHashCode()
        {
            return (nome + "|" + rarita).GetHashCode();
        }
        
    }
    

}
