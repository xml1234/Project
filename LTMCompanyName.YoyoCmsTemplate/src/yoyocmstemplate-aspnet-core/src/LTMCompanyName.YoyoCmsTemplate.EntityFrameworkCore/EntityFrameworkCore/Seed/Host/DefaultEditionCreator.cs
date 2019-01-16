using System.Linq;
using Abp.Application.Editions;
using Abp.Application.Features;
using Microsoft.EntityFrameworkCore;
using LTMCompanyName.YoyoCmsTemplate.Editions;
using LTMCompanyName.YoyoCmsTemplate.Features;

namespace LTMCompanyName.YoyoCmsTemplate.EntityFrameworkCore.Seed.Host
{
    public class DefaultEditionCreator
    {
        private readonly YoyoCmsTemplateDbContext _context;

        public DefaultEditionCreator(YoyoCmsTemplateDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateEditions();
        }

        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.IgnoreQueryFilters()
                .FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new SubscribableEdition
                {
                    Name = EditionManager.DefaultEditionName,
                    DisplayName = EditionManager.DefaultEditionName
                };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                /* Add desired features to the standard edition, if wanted... */
            }

            if (defaultEdition.Id > 0)
            {
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.ChatFeature, true);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToTenantChatFeature, true);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToHostChatFeature, true);
            }
        }

        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            if (_context.EditionFeatureSettings.IgnoreQueryFilters()
                .Any(ef => ef.EditionId == editionId && ef.Name == featureName)) return;

            _context.EditionFeatureSettings.Add(new EditionFeatureSetting
            {
                Name = featureName,
                Value = isEnabled.ToString(),
                EditionId = editionId
            });
            _context.SaveChanges();
        }
    }
}