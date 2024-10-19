import fs from "fs";
import SaxonJs from "saxon-js";

const dirGame = "D:/Hry/steamapps/common/SpaceEngineers/Content/";
const dirDefs = "Data/CubeBlocks/";
const fileGroups = "Data/BlockVariantGroups.sbc";

const configStr = fs.readFileSync("ColorfulItems/Data/Scripts/Sisk/Config.cs").toString();
const filesDefs = fs.readdirSync(`${dirGame}Data/CubeBlocks`, { recursive: true });

/** @typedef {{ id: ?string, icon: ?string }} IconEntry */
/** @type {IconEntry[]} */
let textureDefs = [];

for(let file of filesDefs) {
	if(!file.endsWith(".sbc")) continue;

	const doc = await SaxonJs.getResource({
		location: "file://" + dirGame + dirDefs + file,
		type: "xml"
	});

	const newTexDefs = SaxonJs.XPath.evaluate("/Definitions/CubeBlocks/Definition", doc)?.map(t => {
		return {
			id: SaxonJs.XPath.evaluate("Id/TypeId/text()", t)?.data?.replace(/^MyObjectBuilder_/, "") + "/" + SaxonJs.XPath.evaluate("Id/SubtypeId/text()", t)?.data,
			icon: SaxonJs.XPath.evaluate("Icon/text()", t)?.data?.replaceAll("\\", "/")
		};
	});
	const newGrpDefs = SaxonJs.XPath.evaluate("/Definitions/BlockVariantGroups/BlockVariantGroup", doc)?.map(t => {
		return {
			id: SaxonJs.XPath.evaluate("Id/TypeId/text()", t)?.data?.replace(/^MyObjectBuilder_/, "") + "/" + SaxonJs.XPath.evaluate("Id/SubtypeId/text()", t)?.data,
			icon: SaxonJs.XPath.evaluate("Icon/text()", t)?.data?.replaceAll("\\", "/")
		};
	});

	if(newTexDefs instanceof Array)
		textureDefs.push(...newTexDefs);
	if(newGrpDefs instanceof Array)
		groupDefs.push(...newGrpDefs);
}


const doc = await SaxonJs.getResource({
	location: "file://" + dirGame + fileGroups,
	type: "xml"
});

/** @type {IconEntry[]} */
let groupDefs = SaxonJs.XPath.evaluate("/Definitions/BlockVariantGroups/BlockVariantGroup", doc)?.map(t => {
	return {
		id: SaxonJs.XPath.evaluate("Id/@Type", t)?.value?.replace(/^MyObjectBuilder_/, "") + "/" + SaxonJs.XPath.evaluate("Id/@Subtype", t)?.value,
		icon: SaxonJs.XPath.evaluate("Icon/text()", t)?.data?.replaceAll("\\", "/")
	};
}) ?? [];

/**
 * 
 * @param {IconEntry} item 
 */
function iconFilter(item) {
	return item.id && item.icon
		&& !item.icon.endsWith("/Fake.dds")
		&& !item.icon.includes("/NeonTubes")
		&& !configStr.includes(`"MyObjectBuilder_${item.id}"`);
}

textureDefs = textureDefs.filter(iconFilter);
groupDefs = groupDefs.filter(iconFilter);

console.log("New textures:", textureDefs.length);
console.log();

const command = "texconv.exe -ft png -o ./new/ ";
console.log("Convert commands:");
console.log(textureDefs.map(t => (command + dirGame + t.icon).replaceAll("/", "\\")).join("\n"));
console.log();

console.log("Definitions:");
console.log(textureDefs.map(t => `{ "MyObjectBuilder_${t.id}", "${t.icon}" },`).join("\n"));
console.log("Blueprint Definitions:");
console.log();
console.log(textureDefs.map(t => `{ "MyObjectBuilder_BlueprintDefinition/${t.id}", "${t.icon}" },`).join("\n"));
console.log("Group Definitions:");
console.log();
console.log(groupDefs.map(t => `{ "MyObjectBuilder_${t.id}", "${t.icon}" },`).join("\n"));
