using Abp.Modules;
using Abp.Reflection.Extensions;

namespace YoYo.ABP.Common
{
    public class YoYoABPCommonModule: AbpModule
    {
        public YoYoABPCommonModule()
        {
          
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(YoYoABPCommonModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
        }

        public override void PostInitialize()
        {
           
        }

        public override void PreInitialize()
        {
            
        }
    }
}
