#nullable disable

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace App.Persistence.ContractDefinition;

public partial class VotingDbDeployment : VotingDbDeploymentBase
{
    public VotingDbDeployment() : base(BYTECODE) { }
    public VotingDbDeployment(string byteCode) : base(byteCode) { }
}

public class VotingDbDeploymentBase : ContractDeploymentMessage
{
    public static string BYTECODE = "60c0604052601360809081527f4372656174696f6e44617461496e76616c69640000000000000000000000000060a05260009061003c9082610303565b5034801561004957600080fd5b5060405161095a38038061095a83398101604081905261006891610533565b6100748484848461016b565b600180546001600160a01b0319163317905560026100928482610303565b50600361009f8382610303565b5060005b8451811015610161576100d48282815181106100c1576100c161064a565b602002602001015161021860201b60201c565b43306001600160a01b03168683815181106100f1576100f161064a565b602002602001015163ffffffff167f9152c441005ab366e5c1f46b5e050a35b54700a65bc3c5e11110ae56d9647b718585815181106101325761013261064a565b60200260200101516040516101479190610660565b60405180910390a48061015981610693565b9150506100a3565b5050505050610745565b6000845111801561017d575060008151115b801561018a575083518151145b80156101b957506040805160208082019092526000905283519084012060008051602061093a83398151915214155b80156101e857506040805160208082019092526000905282519083012060008051602061093a83398151915214155b6000906102115760405162461bcd60e51b815260040161020891906106ba565b60405180910390fd5b5050505050565b60408051602080820190925260009081905282519183019190912060008051602061093a833981519152036102605760405162461bcd60e51b815260040161020891906106ba565b5050565b634e487b7160e01b600052604160045260246000fd5b600181811c9082168061028e57607f821691505b6020821081036102ae57634e487b7160e01b600052602260045260246000fd5b50919050565b601f8211156102fe57600081815260208120601f850160051c810160208610156102db5750805b601f850160051c820191505b818110156102fa578281556001016102e7565b5050505b505050565b81516001600160401b0381111561031c5761031c610264565b6103308161032a845461027a565b846102b4565b602080601f831160018114610365576000841561034d5750858301515b600019600386901b1c1916600185901b1785556102fa565b600085815260208120601f198616915b8281101561039457888601518255948401946001909101908401610375565b50858210156103b25787850151600019600388901b60f8161c191681555b5050505050600190811b01905550565b604051601f8201601f191681016001600160401b03811182821017156103ea576103ea610264565b604052919050565b60006001600160401b0382111561040b5761040b610264565b5060051b60200190565b60005b83811015610430578181015183820152602001610418565b50506000910152565b600082601f83011261044a57600080fd5b81516001600160401b0381111561046357610463610264565b610476601f8201601f19166020016103c2565b81815284602083860101111561048b57600080fd5b61049c826020830160208701610415565b949350505050565b600082601f8301126104b557600080fd5b815160206104ca6104c5836103f2565b6103c2565b82815260059290921b840181019181810190868411156104e957600080fd5b8286015b848110156105285780516001600160401b0381111561050c5760008081fd5b61051a8986838b0101610439565b8452509183019183016104ed565b509695505050505050565b6000806000806080858703121561054957600080fd5b84516001600160401b038082111561056057600080fd5b818701915087601f83011261057457600080fd5b815160206105846104c5836103f2565b82815260059290921b8401810191818101908b8411156105a357600080fd5b948201945b838610156105d457855163ffffffff811681146105c55760008081fd5b825294820194908201906105a8565b918a01519198509093505050808211156105ed57600080fd5b6105f988838901610439565b9450604087015191508082111561060f57600080fd5b61061b88838901610439565b9350606087015191508082111561063157600080fd5b5061063e878288016104a4565b91505092959194509250565b634e487b7160e01b600052603260045260246000fd5b602081526000825180602084015261067f816040850160208701610415565b601f01601f19169190910160400192915050565b6000600182016106b357634e487b7160e01b600052601160045260246000fd5b5060010190565b60006020808352600084546106ce8161027a565b808487015260406001808416600081146106ef576001811461070957610737565b60ff1985168984015283151560051b890183019550610737565b896000528660002060005b8581101561072f5781548b8201860152908301908801610714565b8a0184019650505b509398975050505050505050565b6101e6806107546000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c80630121e617146100465780630ae50a3914610064578063c699be6e1461007f575b600080fd5b61004e610087565b60405161005b9190610128565b60405180910390f35b6001546040516001600160a01b03909116815260200161005b565b61004e610119565b60606002805461009690610176565b80601f01602080910402602001604051908101604052809291908181526020018280546100c290610176565b801561010f5780601f106100e45761010080835404028352916020019161010f565b820191906000526020600020905b8154815290600101906020018083116100f257829003601f168201915b5050505050905090565b60606003805461009690610176565b600060208083528351808285015260005b8181101561015557858101830151858201604001528201610139565b506000604082860101526040601f19601f8301168501019250505092915050565b600181811c9082168061018a57607f821691505b6020821081036101aa57634e487b7160e01b600052602260045260246000fd5b5091905056fea26469706673582212206c4b964b680e13583984fdb6013b594ef21189f688cee3a462ffa085198e4c0964736f6c63430008130033c5d2460186f7233c927e7db2dcc703c0e500b653ca82273b7bfad8045d85a470";
    public VotingDbDeploymentBase() : base(BYTECODE) { }
    public VotingDbDeploymentBase(string byteCode) : base(byteCode) { }
    [Parameter("uint32[]", "Sections", 1)]
    public virtual List<uint> Sections { get; set; }
    [Parameter("string", "Timestamp", 2)]
    public virtual string Timestamp { get; set; }
    [Parameter("string", "CompressedSectionData", 3)]
    public virtual string CompressedSectionData { get; set; }
    [Parameter("string[]", "SectionJSON", 4)]
    public virtual List<string> SectionJSON { get; set; }
}

public partial class GetCompressedDataFunction : GetCompressedDataFunctionBase { }

[Function("GetCompressedData", "string")]
public class GetCompressedDataFunctionBase : FunctionMessage
{

}

public partial class GetOwnerFunction : GetOwnerFunctionBase { }

[Function("GetOwner", "address")]
public class GetOwnerFunctionBase : FunctionMessage
{

}

public partial class GetTimestampFunction : GetTimestampFunctionBase { }

[Function("GetTimestamp", "string")]
public class GetTimestampFunctionBase : FunctionMessage
{

}

public partial class SectionEventDTO : SectionEventDTOBase { }

[Event("section")]
public class SectionEventDTOBase : IEventDTO
{
    [Parameter("uint32", "Section", 1, true )]
    public virtual uint Section { get; set; }
    [Parameter("address", "ContractAddress", 2, true )]
    public virtual string ContractAddress { get; set; }
    [Parameter("uint256", "Block", 3, true )]
    public virtual BigInteger Block { get; set; }
    [Parameter("string", "SectionJSON", 4, false )]
    public virtual string SectionJSON { get; set; }
}

public partial class GetCompressedDataOutputDTO : GetCompressedDataOutputDTOBase { }

[FunctionOutput]
public class GetCompressedDataOutputDTOBase : IFunctionOutputDTO 
{
    [Parameter("string", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class GetOwnerOutputDTO : GetOwnerOutputDTOBase { }

[FunctionOutput]
public class GetOwnerOutputDTOBase : IFunctionOutputDTO 
{
    [Parameter("address", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class GetTimestampOutputDTO : GetTimestampOutputDTOBase { }

[FunctionOutput]
public class GetTimestampOutputDTOBase : IFunctionOutputDTO 
{
    [Parameter("string", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}
