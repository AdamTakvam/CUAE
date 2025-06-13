<?

require_once("init.php");

$status = '0';
if (file_exists(SCRIPTS_ROOT . "/rebooting"))
    $status = '1';

// Turn off caching
header("Expires: Mon, 26 Jul 1997 05:00:00 GMT");
header("Last-Modified: " . gmdate("D, d M Y H:i:s") . " GMT");
header("Cache-Control: no-store, no-cache, must-revalidate");
header("Cache-Control: post-check=0, pre-check=0", false);
header("Pragma: no-cache");

header("Content-type: text/xml");
echo '<?xml version="1.0" encoding="utf-8" ?>';
echo '<reboot><status>' . $status . '</status></reboot>';

?>