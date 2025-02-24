using AutoMapper;
using Lojinha.Domain;
using Lojinha.Models;

namespace Lojinha
{
    public static class ModelMapperExtensions
    {
        private static readonly IMapper Mapper;

        static ModelMapperExtensions()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<PedidoModel, Pedido>().ReverseMap().ForMember(d => d.Teste, opt => opt.Ignore());
                c.CreateMap<ProdutosModel, Produtos>().ReverseMap();
                c.CreateMap<EstoqueModel, Estoque>().ReverseMap();
                c.CreateMap<FornecedoresModel, Fornecedores>().ReverseMap();
                c.CreateMap<ItemsPedidosModel, ItemsPedidos>().ReverseMap();
                c.CreateMap<MovimentacaoEstoqueModel, MovimentacaoEstoque>().ReverseMap();
                c.CreateMap<UsuarioModel, Usuario>().ReverseMap();
            });
            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }

        #region Pedido
        public static Pedido ToDomain(this PedidoModel model)
        {
            return Mapper.Map<Pedido>(model);
        }
        public static PedidoModel ToModel(this Pedido domain)
        {
            return Mapper.Map<PedidoModel>(domain);
        }
        #endregion

        #region Produtos
        public static Produtos ToDomain(this ProdutosModel model)
        {
            return Mapper.Map<Produtos>(model);
        }
        public static ProdutosModel ToModel(this Produtos domain)
        {
            return Mapper.Map<ProdutosModel>(domain);
        }
        #endregion

        #region Estoque
        public static Estoque ToDomain(this EstoqueModel model)
        {
            return Mapper.Map<Estoque>(model);
        }
        public static EstoqueModel ToModel(this Estoque domain)
        {
            return Mapper.Map<EstoqueModel>(domain);
        }
        #endregion

        #region Fornecedores
        public static Fornecedores ToDomain(this FornecedoresModel model)
        {
            return Mapper.Map<Fornecedores>(model);
        }
        public static FornecedoresModel ToModel(this Fornecedores domain)
        {
            return Mapper.Map<FornecedoresModel>(domain);
        }
        #endregion

        #region ItemsPedidos
        public static ItemsPedidos ToDomain(this ItemsPedidosModel model)
        {
            return Mapper.Map<ItemsPedidos>(model);
        }
        public static ItemsPedidosModel ToModel(this ItemsPedidos domain)
        {
            return Mapper.Map<ItemsPedidosModel>(domain);
        }
        #endregion

        #region MovimentacaoEstoque
        public static MovimentacaoEstoque ToDomain(this MovimentacaoEstoqueModel model)
        {
            return Mapper.Map<MovimentacaoEstoque>(model);
        }
        public static MovimentacaoEstoqueModel ToModel(this MovimentacaoEstoque domain)
        {
            return Mapper.Map<MovimentacaoEstoqueModel>(domain);
        }
        #endregion

        #region Usuario
        public static Usuario ToDomain(this UsuarioModel model)
        {
            return Mapper.Map<Usuario>(model);
        }
        public static UsuarioModel ToModel(this Usuario domain)
        {
            return Mapper.Map<UsuarioModel>(domain);
        }
        #endregion
    }
}
