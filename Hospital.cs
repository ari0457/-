class Hospital
{
    public Patient[] Patients { get; set; }

    public Patient[] GetPatientsByDiagnosis(string diagnosis)
    {
        int count = 0;
        for (int i = 0; i < Patients.Length; i++)
        {
            if (Patients[i].Diagnosis == diagnosis)
                count++;
        }
        Patient[] result = new Patient[count];
        int index = 0;
        for (int i = 0; i < Patients.Length; i++)
        {
            if (Patients[i].Diagnosis == diagnosis)
            {
                result[index] = Patients[i];
                index++;
            }
        }
        return result;
    }

    public Patient GetOldestPatient()
    {
        Patient oldest = Patients[0];
        for (int i = 1; i < Patients.Length; i++)
        {
            if (Patients[i].Age > oldest.Age)
            {
                oldest = Patients[i];
            }
        }
        return oldest;
    }
}
