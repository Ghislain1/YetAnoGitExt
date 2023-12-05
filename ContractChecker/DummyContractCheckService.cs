using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ContractChecker
{
    public class DummyContractCheckService : IContractCheckService
    {
        private readonly ObservableCollection<ContractRule> _rules = new();
        public IList<ContractRule> Rules => _rules;

        public string[] Lines { get; } = GenerateDummyLines().ToArray();

        public async Task<ContractResult[]> ValidateContractAsync(ContractRule[] rules,string[] lines)
        {
            await Task.Delay(2500);
            return GenerateDummyResult(rules, lines).ToArray();
        }

        private IEnumerable<ContractResult> GenerateDummyResult(ContractRule[] rules, string[] lines)
        {
            Random rand = new();

            return rules.Select(x =>
            {
                var id = rand.Next()%lines.Length;
                var result = (ResultStatus)(rand.Next() % Enum.GetValues(typeof(ResultStatus)).Length);

                return new ContractResult(x.Name, id, result);
            });
        }

        private static IEnumerable<string> GenerateDummyLines()
        {
            yield return "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Faucibus purus in massa tempor nec. Turpis tincidunt id aliquet risus feugiat in. Fermentum odio eu feugiat pretium. Egestas egestas fringilla phasellus faucibus scelerisque eleifend donec pretium. Tincidunt tortor aliquam nulla facilisi cras fermentum odio. Platea dictumst vestibulum rhoncus est pellentesque elit ullamcorper dignissim cras. Cras pulvinar mattis nunc sed blandit libero. Egestas dui id ornare arcu odio ut. Tincidunt nunc pulvinar sapien et. Ipsum a arcu cursus vitae. Condimentum mattis pellentesque id nibh tortor id aliquet lectus. Sit amet massa vitae tortor. Sem fringilla ut morbi tincidunt augue interdum velit euismod in. Euismod nisi porta lorem mollis. Posuere ac ut consequat semper viverra. Mauris pharetra et ultrices neque ornare. Vulputate sapien nec sagittis aliquam malesuada bibendum arcu vitae. Eu volutpat odio facilisis mauris sit amet massa vitae tortor. Tortor at auctor urna nunc id cursus metus aliquam eleifend.";
            yield return "Non nisi est sit amet facilisis magna. At varius vel pharetra vel turpis nunc eget lorem. Amet cursus sit amet dictum sit amet. Luctus venenatis lectus magna fringilla urna porttitor rhoncus dolor. Ipsum dolor sit amet consectetur adipiscing elit pellentesque. Sit amet nulla facilisi morbi tempus iaculis. Eu turpis egestas pretium aenean pharetra magna ac placerat. Fermentum iaculis eu non diam phasellus vestibulum. Elementum facilisis leo vel fringilla est ullamcorper eget nulla facilisi. Pharetra magna ac placerat vestibulum lectus mauris ultrices eros in. Cras tincidunt lobortis feugiat vivamus at. Lacinia at quis risus sed vulputate odio ut. Ullamcorper eget nulla facilisi etiam. Tempus iaculis urna id volutpat lacus laoreet. Sed cras ornare arcu dui vivamus arcu felis bibendum. Tellus id interdum velit laoreet id donec. Auctor eu augue ut lectus arcu bibendum at varius vel. Congue mauris rhoncus aenean vel elit. Ante metus dictum at tempor commodo ullamcorper a.";
            yield return "Sed lectus vestibulum mattis ullamcorper. Enim tortor at auctor urna nunc id cursus. Enim blandit volutpat maecenas volutpat blandit aliquam etiam erat. At auctor urna nunc id cursus metus aliquam. Vel elit scelerisque mauris pellentesque pulvinar pellentesque. Enim eu turpis egestas pretium aenean pharetra magna ac. Laoreet id donec ultrices tincidunt arcu non sodales. Orci eu lobortis elementum nibh tellus molestie nunc non blandit. Pharetra pharetra massa massa ultricies mi quis. Turpis egestas pretium aenean pharetra. Id interdum velit laoreet id donec ultrices tincidunt arcu non.";
            yield return "Netus et malesuada fames ac turpis egestas. Egestas pretium aenean pharetra magna ac. Mauris a diam maecenas sed enim ut sem viverra. Amet est placerat in egestas erat. Suscipit adipiscing bibendum est ultricies integer quis auctor. At ultrices mi tempus imperdiet nulla malesuada pellentesque. Tellus elementum sagittis vitae et leo. Vivamus at augue eget arcu dictum varius. Tempus urna et pharetra pharetra massa massa ultricies mi. Dolor sit amet consectetur adipiscing elit ut aliquam. A iaculis at erat pellentesque adipiscing commodo elit. Neque sodales ut etiam sit amet. Euismod quis viverra nibh cras pulvinar. Pretium nibh ipsum consequat nisl vel pretium lectus quam id. Vestibulum mattis ullamcorper velit sed ullamcorper morbi tincidunt ornare. Quis ipsum suspendisse ultrices gravida dictum fusce ut placerat orci. Eget lorem dolor sed viverra. Risus sed vulputate odio ut enim blandit volutpat. Ut etiam sit amet nisl purus in.";
            yield return "Duis convallis convallis tellus id interdum velit laoreet id donec. Vulputate sapien nec sagittis aliquam malesuada bibendum. A cras semper auctor neque vitae tempus quam pellentesque. Id neque aliquam vestibulum morbi. Nisl tincidunt eget nullam non nisi. Facilisis gravida neque convallis a. In nisl nisi scelerisque eu ultrices vitae auctor. Sagittis eu volutpat odio facilisis mauris sit amet massa. Mi proin sed libero enim sed faucibus turpis in eu. Fames ac turpis egestas maecenas pharetra convallis posuere. Ultricies leo integer malesuada nunc vel risus commodo viverra maecenas. Massa tincidunt dui ut ornare lectus. Pharetra massa massa ultricies mi quis hendrerit dolor. Netus et malesuada fames ac turpis egestas sed tempus. Lacus laoreet non curabitur gravida arcu ac tortor. Pulvinar elementum integer enim neque volutpat ac tincidunt vitae. Sem nulla pharetra diam sit amet nisl suscipit adipiscing.";
            yield return "Cursus in hac habitasse platea dictumst. Volutpat commodo sed egestas egestas. Eget nulla facilisi etiam dignissim diam. Quam adipiscing vitae proin sagittis nisl rhoncus mattis rhoncus urna. Id diam vel quam elementum pulvinar etiam. Sit amet purus gravida quis blandit turpis cursus in hac. Odio euismod lacinia at quis risus sed vulputate. Fermentum leo vel orci porta non pulvinar neque. Faucibus interdum posuere lorem ipsum dolor sit amet consectetur. Enim facilisis gravida neque convallis a. Nulla pharetra diam sit amet nisl suscipit adipiscing bibendum. Rhoncus dolor purus non enim praesent elementum facilisis. Porta non pulvinar neque laoreet. Vitae congue eu consequat ac felis donec et odio pellentesque.";
            yield return "Semper auctor neque vitae tempus quam pellentesque nec. Velit sed ullamcorper morbi tincidunt ornare massa. Sagittis vitae et leo duis ut diam. Laoreet non curabitur gravida arcu ac tortor dignissim convallis aenean. Phasellus egestas tellus rutrum tellus pellentesque eu tincidunt. Facilisis magna etiam tempor orci eu lobortis elementum nibh. Tempor id eu nisl nunc mi ipsum faucibus. Tortor dignissim convallis aenean et tortor at. Amet volutpat consequat mauris nunc congue nisi vitae. Suspendisse in est ante in nibh mauris cursus mattis molestie. Malesuada proin libero nunc consequat interdum. Aliquet sagittis id consectetur purus ut. Nisl nisi scelerisque eu ultrices vitae auctor eu augue. Justo laoreet sit amet cursus sit amet dictum sit. Tellus pellentesque eu tincidunt tortor aliquam. Morbi tincidunt ornare massa eget egestas purus viverra. Amet mattis vulputate enim nulla aliquet porttitor. Eget gravida cum sociis natoque penatibus. Donec et odio pellentesque diam volutpat commodo sed egestas. Mi ipsum faucibus vitae aliquet.";
            yield return "Adipiscing tristique risus nec feugiat in fermentum posuere. Volutpat lacus laoreet non curabitur gravida arcu ac tortor. Non nisi est sit amet facilisis magna etiam tempor. Posuere urna nec tincidunt praesent semper feugiat nibh. Risus viverra adipiscing at in tellus. Imperdiet sed euismod nisi porta lorem mollis aliquam ut porttitor. Nunc lobortis mattis aliquam faucibus purus in massa tempor. Eu scelerisque felis imperdiet proin fermentum leo vel. Dictum varius duis at consectetur lorem. Nullam non nisi est sit amet facilisis magna. Cursus risus at ultrices mi tempus. Tristique et egestas quis ipsum. Ac ut consequat semper viverra nam. In hac habitasse platea dictumst vestibulum. Enim ut sem viverra aliquet.";
            yield return "Et malesuada fames ac turpis egestas maecenas pharetra convallis posuere. Vel facilisis volutpat est velit egestas dui id ornare arcu. Nulla facilisi nullam vehicula ipsum a arcu. At volutpat diam ut venenatis. Scelerisque fermentum dui faucibus in. Non arcu risus quis varius. Augue ut lectus arcu bibendum at. Id venenatis a condimentum vitae. Lacus vel facilisis volutpat est. Aliquet nec ullamcorper sit amet risus nullam eget felis eget. Mattis pellentesque id nibh tortor id aliquet lectus proin. Morbi tincidunt augue interdum velit euismod in pellentesque. Id venenatis a condimentum vitae. Scelerisque purus semper eget duis at. Dictum non consectetur a erat nam at.";
            yield return "Imperdiet proin fermentum leo vel. Integer malesuada nunc vel risus commodo viverra. Faucibus interdum posuere lorem ipsum. Tincidunt id aliquet risus feugiat. Dui vivamus arcu felis bibendum ut tristique et egestas quis. Tincidunt ornare massa eget egestas purus viverra accumsan in. Mauris pharetra et ultrices neque ornare. Ullamcorper sit amet risus nullam eget felis eget. Ac tincidunt vitae semper quis lectus nulla at. Vitae semper quis lectus nulla at. Sed faucibus turpis in eu mi bibendum.";
        }

    }
}
