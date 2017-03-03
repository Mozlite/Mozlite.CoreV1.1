namespace Mozlite.Core
{
    /// <summary>
    /// 服务接口。
    /// </summary>
    public interface IService
    {

    }

    /// <summary>
    /// 单例服务接口。
    /// </summary>
    public interface ISingletonService : IService { }

    /// <summary>
    /// [上下文单例]需要注册的依赖项接口。
    /// </summary>
    public interface IScopedService : IService
    {

    }

    /// <summary>
    /// 服务接口。
    /// </summary>
    public interface IServices : IService
    {

    }

    /// <summary>
    /// 单例服务接口。
    /// </summary>
    public interface ISingletonServices : IService
    {

    }

    /// <summary>
    /// [上下文单例]需要注册的依赖项接口。
    /// </summary>
    public interface IScopedServices : IService
    {

    }
}