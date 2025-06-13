<?php

abstract class ApplicationConfigUtils
{
    static function GetCodecList()
    {
        return array('G.711u_20ms','G.711u_30ms','G.711a_20ms','G.711a_30ms','G.723.1_30ms','G.723.1_60ms','G.729a_20ms','G.729a_30ms','G.729a_40ms');
    }
}

?>