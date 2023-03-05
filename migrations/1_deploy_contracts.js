// const ConvertLib = artifacts.require("ConvertLib");
// const MetaCoin = artifacts.require("MetaCoin");
const VotingDb = artifacts.require("VotingDb");
const seedData = require('../contracts/SeedData.js');
// const SeedData = require('../contracts/SeedData.js');

module.exports = function(deployer) {
  deployer.deploy(VotingDb, seedData["votes"], seedData["candidates"], seedData["sections"], seedData["timestamp"]);
};

// module.exports = function(deployer) {
//   // deployer.deploy(ConvertLib);
//   // deployer.link(ConvertLib, MetaCoin);
//   // deployer.deploy(MetaCoin);
//   // deployer.deploy(VotingDb, seedData["votes"], seedData["candidates"], seedData["sections"], seedData["timestamp"]);
//   const votes = [
//     [3, 8, 5, 6],
//     [1, 0, 7, 4],
//     [2, 9, 0, 1],
//     [7, 8, 5, 7],
//     [3, 5, 4, 8],
//     [9, 0, 2, 4],
//     [2, 6, 4, 9],
//     [4, 7, 4, 2],
//     [7, 9, 5, 1],
//     [6, 8, 3, 7],
//     [3, 8, 9, 5],
//     [9, 6, 2, 3],
//     [4, 1, 8, 9],
//     [1, 7, 5, 2],
//     [9, 5, 4, 6],
//     [6, 0, 7, 3],
//     [2, 4, 6, 1],
//     [3, 2, 9, 8],
//     [1, 8, 7, 5],
//     [7, 6, 1, 9],
//     [8, 3, 0, 4],
//     [5, 9, 8, 6],
//     [4, 5, 1, 7],
//     [8, 4, 2, 3],
//     [6, 7, 3, 0],
//     [2, 1, 5, 9],
//     [9, 3, 6, 4],
//     [5, 0, 8, 2],
//     [4, 2, 7, 1],
//     [7, 1, 0, 8]
//   ];
//   const candidates = ["John", "Mark", "David", "Paul"];
//   const sections = 
//   [
//     10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 
//     120, 130, 140, 150, 160, 170, 180, 190, 200, 210,
//     220, 230, 240, 250, 260, 270, 280, 290, 300
//   ];
//   const timestamp = "2020-04-10T17:50:00Z";

//   deployer.deploy(VotingDb, votes, candidates, sections, timestamp);
// };
