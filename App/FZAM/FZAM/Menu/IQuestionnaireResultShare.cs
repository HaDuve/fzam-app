using System.Threading.Tasks;

namespace FZAM.Menu
{
    public interface IQuestionnaireResultShare
    {
        Task Show(string resultAsJson, string lang, string version);
    }
}