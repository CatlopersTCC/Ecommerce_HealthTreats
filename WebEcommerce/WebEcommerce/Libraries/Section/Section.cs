namespace WebEcommerce.Libraries.Section
{
    public class Section
    {
        IHttpContextAccessor _context;
        public Section(IHttpContextAccessor context)
        {
            _context = context;
        }

        //Criação do CRUD (Cadastrar, Atualizar, Consultar e Remover) da sessão
        //Cadastrar uma nova sessão
        public void Cadastrar(string Key, string Valor)
        {
            _context.HttpContext.Session.SetString(Key, Valor);
        }

        //Consultar uma sessão a partir de sua chave
        public string Consultar(string Key)
        {
            return _context.HttpContext.Session.GetString(Key);
        }

        //Verificar se a sessão existe
        public bool Existe(string Key)
        {
            if(_context.HttpContext.Session.GetString(Key) == null)
            {
                return false;
            }
            return true;
        }

        //Remover a sessão pela sua chave
        public void Remover(string Key)
        {
            _context.HttpContext.Session.Remove(Key);
        }
        //Remover todas as sessões
        public void RemoverTodas()
        {
            _context.HttpContext.Session.Clear();
        }

        //Atualizar uma sessão existente
        public void Atualizar(string Key, string Valor)
        {
            if (Existe(Key))
            {
                _context.HttpContext.Session.Remove(Key);
            }
            _context.HttpContext.Session.SetString(Key, Valor);
        }
    }
}
