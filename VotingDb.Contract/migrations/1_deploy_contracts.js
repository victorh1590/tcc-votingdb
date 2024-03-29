const VotingDb = artifacts.require("VotingDb");
const seedData = require('../contracts/SeedData.js');
const compressedData = require('../contracts/CompressedData.js');

module.exports = function(deployer) {
  deployer.deploy(VotingDb, 
    seedData["votes"],
    seedData["candidates"],
    seedData["sections"],
    seedData["timestamp"], 
    compressedData["compressedSectionData"]);
};