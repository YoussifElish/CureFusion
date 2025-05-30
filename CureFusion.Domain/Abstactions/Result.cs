﻿namespace CureFusion.Domain.Abstactions;

public class Result
{
    public Result(bool Issuccess, Error error)
    {
        if ((Issuccess && error != Error.None) || (!Issuccess && error == Error.None))
            throw new InvalidOperationException();
        {
            IsSuccess = Issuccess;
            Error = error;
        }
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; } = default!;

    public static Result Success() => new(true, Error.None);
    public static Result<Tvalue> Success<Tvalue>(Tvalue value) => new(value, true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<Tvalue> Failure<Tvalue>(Error error) => new(default, false, error);


}
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool Issuccess, Error error) : base(Issuccess, error)
    {
        _value = value;
    }
    public TValue? Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("failure results can't have value !");
}
