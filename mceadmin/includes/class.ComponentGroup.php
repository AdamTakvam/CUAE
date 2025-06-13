<?php

/* ComponentGroup class handles actions related to a group of any components.
 * These groups associations are stored in the mce_component_groups and
 * mce_component_group_members tables.
 */

require_once("common.php");

class ComponentGroup
{

/* PRVIATE METHODS */

    private $mId;           // Id number of the group
    private $mDb;           // Database object
    private $mData;         // Hash of details associated with the group
    private $mComponents;   // Array of ids for components in the group


/* PUBLIC METHODS */

    public function __construct()
    // Initialize the object
    {
        $this->mDb = new MceDb();
        $this->mId = NULL;
        $this->mType = NULL;
        $this->mData = array();
        $this->mComponents = array();
    }

    public function SetId($id)
    // Set the id number of the group to retrieve
    {
        if (NULL == $this->mId)
        {
            $this->mId = $id;
            return TRUE;
        }
        return FALSE;
    }

    public function Build()
    // Retrive the group's data from the database
    {
        if (array() == $this->mData && NULL != $this->mId)
        {
            $this->mData = $this->mDb->GetRow("SELECT * FROM mce_component_groups WHERE mce_component_groups_id = ?", array($this->mId));
            $this->mType = $this->mData['component_type'];
            return TRUE;
        }
        return FALSE;
    }


    // Various return functions for group data

    public function GetType() { return $this->mType; }

    public function GetId() { return $this->mData['mce_component_groups_id']; }

    public function GetName() { return $this->mData['name']; }

    public function GetDescription() { return $this->mData['description']; }

    public function GetAlarmId() { return $this->mData['mce_alarm_group_id']; }

    public function GetFailoverId() { return $this->mData['mce_failover_group_id']; }

    public function IsDefault() { return $this->mData['default_group']; }

    // Various functions for setting group data

    public function SetName($name) { $this->mData['name'] = $name; }

    public function SetDescription($description) { $this->mData['description'] = $description; }

    public function SetAlarmId($id) { $this->mData['mce_alarm_group_id'] = $id; }

    public function SetFailoverId($id) { $this->mData['mce_failover_group_id'] = $id; }


    public function Update()
    // Updates the group's details with the data internally set
    {
        $this->mDb->StartTrans();
        $this->mDb->Execute("UPDATE mce_component_groups SET name = ?, description = ?, mce_alarm_group_id = ?, mce_failover_group_id = ? " .
                            "WHERE mce_component_groups_id = ?",
                            array(  $this->mData['name'],
                                    $this->mData['description'],
                                    $this->mData['mce_alarm_group_id'],
                                    $this->mData['mce_failover_group_id'],
                                    $this->mId));
        $this->mDb->CompleteTrans();
        return TRUE;
    }

    public function GetComponents()
    // Returns an array of ids of components in the group
    {
        $this->mComponents = $this->mDb->GetAll("SELECT * FROM mce_component_group_members JOIN mce_components USING (mce_components_id) WHERE mce_component_groups_id = ? ORDER BY ordinal ASC", array($this->mId));
        return $this->mComponents;
    }

    public function AddComponent($component)
    // Adds a component to the group membership.
    // $component may be the id number or the component object itself.
    {
        if (is_object($component))
            $component_id = $component->GetId();
        else
            $component_id = $component;

        $this->GetComponents();
        if (!in_array($component_id, $this->mComponents))
        {
            $this->mDb->StartTrans();
            $last_ord = $this->mDb->GetOne("SELECT MAX(ordinal) FROM mce_component_group_members WHERE mce_component_groups_id = ?", array($this->mId));
            if (!is_numeric($last_ord))
                $ordinal = 0;
            else
                $ordinal = $last_ord + 1;
            $this->mDb->Execute("INSERT INTO mce_component_group_members (mce_components_id, mce_component_groups_id, ordinal) VALUES (?,?,?)",
                                array($component_id, $this->mId, $ordinal));
            EventLog::log(LogMessageType::AUDIT, "Component added to group " . $this->GetName(), LogMessageId::ADDED_TO_GROUP, "Group ID: $this->mId, Component ID: $component_id");
            $this->mDb->CompleteTrans();
            return TRUE;
        }
        return FALSE;
    }

    public function RemoveComponent($component)
    // Removes a component from the group membership.
    // $component may be the id number or the component object itself.
    {
        if (is_object($component))
            $component_id = $component->GetId();
        else
            $component_id = $component;

        $this->mDb->StartTrans();
        $this->mDb->Execute("DELETE FROM mce_component_group_members WHERE mce_components_id = ? AND mce_component_groups_id = ?",
                            array($component_id, $this->mId));
        EventLog::log(LogMessageType::AUDIT, "Component removed from group " . $this->GetName(), LogMessageId::REMOVED_FROM_GROUP,  "Group ID: $this->mId, Component ID: $component_id");
        $this->mDb->CompleteTrans();
        return TRUE;
    }

    public function OrderComponents($components)
    // Changes the ordinal values of the group members.
    // Pass in an array of component ids in the order of which they should be reordered.
    {
        $this->mDb->StartTrans();
        foreach ($components as $key => $id)
        {
            $this->mDb->Execute("UPDATE mce_component_group_members SET ordinal = ? WHERE mce_components_id = ? AND mce_component_groups_id = ?",
                                array($key, $id, $this->mId));
        }
        $this->mDb->CompleteTrans();
    }
    
    public function Delete()
    // Deletes the group and associations from the database
    {
        $this->mDb->StartTrans();
        $this->mDb->Execute("DELETE FROM mce_component_group_members WHERE mce_component_groups_id = ?", array($this->mId));
        $this->mDb->Execute("DELETE FROM mce_users_acl_list WHERE mce_component_groups_id = ?", array($this->mId));
        $this->mDb->Execute("DELETE FROM mce_component_groups WHERE mce_component_groups_id = ?", array($this->mId));
        EventLog::log(LogMessageType::AUDIT, "Group " . $this->GetName() . " was deleted.", LogMessageId::GROUP_DELETED);
        $this->mDb->CompleteTrans();
        return TRUE;
    }

}

?>