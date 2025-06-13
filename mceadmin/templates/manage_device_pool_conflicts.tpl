<p>
There is a conflict in the range of the devices you want to create and already defined devices for the {$type_display}.  You can resolve the conflict in two ways:
</p>

<form action="{$PHP_SELF}" method="post">
    <ul style="list-style:none">
    
        <li>
        <input type="radio" name="resolve_method" value="overlap" checked="checked" /> 
        Create the range of devices as you have defined it and skip over any already existing devices within that range.
        <br />
        This will create only <strong>{$count-$conflict_count} devices</strong>.
        </li>
        
        <li>
        <input type="radio" name="resolve_method" value="append" />
        Create the amount of devices that you have specified but start from after the device name <strong>{$highest_device}</strong>.
        <br />
        The range will end with the device name <strong>{$new_highest_device}</strong>.
        </li>
        
    </ul>
    
    <p>
    <input type="submit" name="submit" value="Resolve Conflict" />
    <input type="submit" name="cancel" value="Cancel Create Devices" />
    </p>
</form>