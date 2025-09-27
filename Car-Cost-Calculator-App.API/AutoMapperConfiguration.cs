using AutoMapper;

namespace Car_Cost_Calculator_App.API
{
    public static class AutoMapperConfiguration
    {
        #region Public Methods

        public static MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return config;
        }

        #endregion
    }
}
