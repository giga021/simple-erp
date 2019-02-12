var fs = require('fs-extra');

fs.moveSync('build', '../wwwroot', { overwrite: true }, err => { console.error(err) });