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
    public VotingDbDeployment() : base(BYTECODE)
    {
    }

    public VotingDbDeployment(string byteCode) : base(byteCode)
    {
    }
}

public class VotingDbDeploymentBase : ContractDeploymentMessage
{
    public static string BYTECODE =
        "60c0604052601360809081527f4372656174696f6e44617461496e76616c69640000000000000000000000000060a05260009061003c908261042d565b5034801561004957600080fd5b50604051610af0380380610af0833981016040819052610068916106c0565b610075838587858561028e565b600180546001600160a01b031916331790556002610093838261042d565b5060036100a0828261042d565b50306001600160a01b03167ff2ac6e4201b33a2adc8f05978437f93583d6513106c60315ddbe8594fe63180f4385876040516100de939291906107d2565b60405180910390a260005b83518110156102835783818151811061010457610104610807565b602002602001015163ffffffff167fb57e5c2e5455a033a3919e6f163d08466f514cc43d0523f9eceb038be76e21433088848151811061014657610146610807565b602002602001015160405161015c92919061081d565b60405180910390a261018d86828151811061017957610179610807565b60200260200101518661036d60201b60201c565b60005b8551811015610270578482815181106101ab576101ab610807565b602002602001015163ffffffff168682815181106101cb576101cb610807565b602002602001015163ffffffff167f99dc85b86c3891df2d92db75bee310d3aa1da03f7b9953decdcaa303163fd5f3308a868151811061020d5761020d610807565b6020026020010151858151811061022657610226610807565b60200260200101516040516102569291906001600160a01b0392909216825263ffffffff16602082015260400190565b60405180910390a38061026881610849565b915050610190565b508061027b81610849565b9150506100e9565b5050505050506108fb565b600085511180156102a0575060008451115b80156102ad575060008551115b80156102ba575084518351145b80156102fb5750604080516020808201909252600090528251908301207fc5d2460186f7233c927e7db2dcc703c0e500b653ca82273b7bfad8045d85a47014155b801561033c5750604080516020808201909252600090528151908201207fc5d2460186f7233c927e7db2dcc703c0e500b653ca82273b7bfad8045d85a47014155b6000906103655760405162461bcd60e51b815260040161035c9190610870565b60405180910390fd5b505050505050565b80518251146000906103925760405162461bcd60e51b815260040161035c9190610870565b505050565b634e487b7160e01b600052604160045260246000fd5b600181811c908216806103c157607f821691505b6020821081036103e157634e487b7160e01b600052602260045260246000fd5b50919050565b601f82111561039257600081815260208120601f850160051c8101602086101561040e5750805b601f850160051c820191505b818110156103655782815560010161041a565b81516001600160401b0381111561044657610446610397565b61045a8161045484546103ad565b846103e7565b602080601f83116001811461048f57600084156104775750858301515b600019600386901b1c1916600185901b178555610365565b600085815260208120601f198616915b828110156104be5788860151825594840194600190910190840161049f565b50858210156104dc5787850151600019600388901b60f8161c191681555b5050505050600190811b01905550565b604051601f8201601f191681016001600160401b038111828210171561051457610514610397565b604052919050565b60006001600160401b0382111561053557610535610397565b5060051b60200190565b600082601f83011261055057600080fd5b815160206105656105608361051c565b6104ec565b82815260059290921b8401810191818101908684111561058457600080fd5b8286015b848110156105b257805163ffffffff811681146105a55760008081fd5b8352918301918301610588565b509695505050505050565b600082601f8301126105ce57600080fd5b815160206105de6105608361051c565b82815260059290921b840181019181810190868411156105fd57600080fd5b8286015b848110156105b25780516001600160401b038111156106205760008081fd5b61062e8986838b010161053f565b845250918301918301610601565b600082601f83011261064d57600080fd5b81516001600160401b0381111561066657610666610397565b602061067a601f8301601f191682016104ec565b828152858284870101111561068e57600080fd5b60005b838110156106ac578581018301518282018401528201610691565b506000928101909101919091529392505050565b600080600080600060a086880312156106d857600080fd5b85516001600160401b03808211156106ef57600080fd5b6106fb89838a016105bd565b9650602088015191508082111561071157600080fd5b61071d89838a0161053f565b9550604088015191508082111561073357600080fd5b61073f89838a0161053f565b9450606088015191508082111561075557600080fd5b61076189838a0161063c565b9350608088015191508082111561077757600080fd5b506107848882890161063c565b9150509295509295909350565b600081518084526020808501945080840160005b838110156107c757815163ffffffff16875295820195908201906001016107a5565b509495945050505050565b8381526060602082015260006107eb6060830185610791565b82810360408401526107fd8185610791565b9695505050505050565b634e487b7160e01b600052603260045260246000fd5b6001600160a01b038316815260406020820181905260009061084190830184610791565b949350505050565b60006001820161086957634e487b7160e01b600052601160045260246000fd5b5060010190565b6000602080835260008454610884816103ad565b808487015260406001808416600081146108a557600181146108bf576108ed565b60ff1985168984015283151560051b8901830195506108ed565b896000528660002060005b858110156108e55781548b82018601529083019088016108ca565b8a0184019650505b509398975050505050505050565b6101e68061090a6000396000f3fe608060405234801561001057600080fd5b50600436106100415760003560e01c80630121e617146100465780630ae50a3914610064578063c699be6e1461007f575b600080fd5b61004e610087565b60405161005b9190610128565b60405180910390f35b6001546040516001600160a01b03909116815260200161005b565b61004e610119565b60606002805461009690610176565b80601f01602080910402602001604051908101604052809291908181526020018280546100c290610176565b801561010f5780601f106100e45761010080835404028352916020019161010f565b820191906000526020600020905b8154815290600101906020018083116100f257829003601f168201915b5050505050905090565b60606003805461009690610176565b600060208083528351808285015260005b8181101561015557858101830151858201604001528201610139565b506000604082860101526040601f19601f8301168501019250505092915050565b600181811c9082168061018a57607f821691505b6020821081036101aa57634e487b7160e01b600052602260045260246000fd5b5091905056fea2646970667358221220191f4ccd854e71570d7e4abbe6dd1d5a640db4afe46f08a79709d2006a390ffa64736f6c63430008130033";

    public VotingDbDeploymentBase() : base(BYTECODE)
    {
    }

    public VotingDbDeploymentBase(string byteCode) : base(byteCode)
    {
    }

    [Parameter("uint32[][]", "Votes", 1)] public virtual List<List<uint>> Votes { get; set; }

    [Parameter("uint32[]", "Candidates", 2)]
    public virtual List<uint> Candidates { get; set; }

    [Parameter("uint32[]", "Sections", 3)] public virtual List<uint> Sections { get; set; }
    [Parameter("string", "Timestamp", 4)] public virtual string Timestamp { get; set; }

    [Parameter("string", "CompressedSectionData", 5)]
    public virtual string CompressedSectionData { get; set; }
}

public partial class GetCompressedDataFunction : GetCompressedDataFunctionBase
{
}

[Function("GetCompressedData", "string")]
public class GetCompressedDataFunctionBase : FunctionMessage
{
}

public partial class GetOwnerFunction : GetOwnerFunctionBase
{
}

[Function("GetOwner", "address")]
public class GetOwnerFunctionBase : FunctionMessage
{
}

public partial class GetTimestampFunction : GetTimestampFunctionBase
{
}

[Function("GetTimestamp", "string")]
public class GetTimestampFunctionBase : FunctionMessage
{
}

public partial class CandidateEventDTO : CandidateEventDTOBase
{
}

[Event("candidate")]
public class CandidateEventDTOBase : IEventDTO
{
    [Parameter("uint32", "Candidate", 1, true)]
    public virtual uint Candidate { get; set; }

    [Parameter("uint32", "Section", 2, true)]
    public virtual uint Section { get; set; }

    [Parameter("address", "ContractAddress", 3, false)]
    public virtual string ContractAddress { get; set; }

    [Parameter("uint32", "Votes", 4, false)]
    public virtual uint Votes { get; set; }
}

public partial class MetadataEventDTO : MetadataEventDTOBase
{
}

[Event("metadata")]
public class MetadataEventDTOBase : IEventDTO
{
    [Parameter("address", "ContractAddress", 1, true)]
    public virtual string ContractAddress { get; set; }

    [Parameter("uint256", "Block", 2, false)]
    public virtual BigInteger Block { get; set; }

    [Parameter("uint32[]", "Sections", 3, false)]
    public virtual List<uint> Sections { get; set; }

    [Parameter("uint32[]", "Candidates", 4, false)]
    public virtual List<uint> Candidates { get; set; }
}

public partial class SectionEventDTO : SectionEventDTOBase
{
}

[Event("section")]
public class SectionEventDTOBase : IEventDTO
{
    [Parameter("uint32", "Section", 1, true)]
    public virtual uint Section { get; set; }

    [Parameter("address", "ContractAddress", 2, false)]
    public virtual string ContractAddress { get; set; }

    [Parameter("uint32[]", "Votes", 3, false)]
    public virtual List<uint> Votes { get; set; }
}

public partial class GetCompressedDataOutputDTO : GetCompressedDataOutputDTOBase
{
}

[FunctionOutput]
public class GetCompressedDataOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("string", "", 1)] public virtual string ReturnValue1 { get; set; }
}

public partial class GetOwnerOutputDTO : GetOwnerOutputDTOBase
{
}

[FunctionOutput]
public class GetOwnerOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("address", "", 1)] public virtual string ReturnValue1 { get; set; }
}

public partial class GetTimestampOutputDTO : GetTimestampOutputDTOBase
{
}

[FunctionOutput]
public class GetTimestampOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("string", "", 1)] public virtual string ReturnValue1 { get; set; }
}