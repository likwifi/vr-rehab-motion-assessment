namespace DefaultNamespace
{
    using UnityEngine;

    // Define a Gender enum for clarity
    public enum Gender
    {
        Male,
        Female
    }

    public static class NameGenerator
    {
        // Define an array of male names (example subset from your original list)
        private static readonly string[] maleNames = new string[]
        {
            "Alex",
            "Chris",
            "Mike",
            "Dan",
            "Matt",
            "Ben",
            "Will",
            "Josh",
            "Ravi",
            "Omar",
            "Luca",
            "Diego",
            "Hans",
            "Elias",
            "Miguel",
            "Arjun",
            "Kofi",
            "Pablo",
            "Noah",
            "Finn",
            "Zane",
            "Oscar",
            "Zion",
            "Liam",
            "Soren",
            "Saul",
            "Leo",
            "Alec",
            "Avi",
            "Milan",
            "Arlo"
            // Add additional male names as needed…
        };

        // Define an array of female names (example subset from your original list)
        private static readonly string[] femaleNames = new string[]
        {
            "Bella",
            "Liz",
            "Sophie",
            "Jess",
            "Liv",
            "Maddie",
            "Mimi",
            "Ana",
            "Keiko",
            "Amina",
            "Yara",
            "Fatima",
            "Zara",
            "Maya",
            "Rosa",
            "Sasha",
            "Leila",
            "Tina",
            "Aiko",
            "Kara",
            "Nina",
            "Lina",
            "Carmen",
            "Isla",
            "Hana",
            "Alya",
            "Ines",
            "Eve",
            "Soraya",
            "Laila",
            "Sana",
            "Aria",
            "Mila",
            "Nadia",
            "Lila",
            "Rina",
            "Esme",
            "Anya",
            "Nicolette",
            "Mei",
            "Ria",
            "Zola",
            "Lara",
            "Nami",
            "Isabel",
            "Uma",
            "Farah",
            "Luz",
            "Lena",
            "Noor",
            "Nia",
            "Tara"
            // Add additional female names as needed…
        };

        /// <summary>
        /// Returns a random name from the combined male and female lists.
        /// </summary>
        public static string GetRandomName()
        {
            // Combine both arrays (you could choose to remove duplicates if required)
            var allNames = new string[maleNames.Length + femaleNames.Length];
            maleNames.CopyTo(allNames, 0);
            femaleNames.CopyTo(allNames, maleNames.Length);
            var index = Random.Range(0, allNames.Length);
            return allNames[index];
        }

        /// <summary>
        /// Returns a random name based on the specified gender.
        /// </summary>
        /// <param name="gender">Use Gender.Male or Gender.Female</param>
        public static string GetRandomName(bool isFemale=false)
        {
            // Choose the array based on the gender parameter.
            var chosenNames = isFemale ? femaleNames:maleNames ;
            var index = Random.Range(0, chosenNames.Length);
            return chosenNames[index];
        }
    }
}
