<?php

/* The PageLogic class creates an object that can be passed a count of items and a page number
 * and can generate the values indicating the amount of pages, the number range of entries
 * corresponding to current page, the next and previous pages (if there any) and can even
 * create the proper SQL LIMIT clause.  Instead of creating accessor methods, a lot of the data
 * exists as public members, and can be more easily accessed from the template.  Page numbers
 * are 1-based.
 *
 * The best way to tak advantage of this object is include page_nav.tpl in your template and
 * assign a PageLogic object to the variable 'page_logic' and, voila, instant page navigation.
 *
 * The reason why this is also concerned with creating proper SQL LIMIT clauses is because
 * it is optimal to pull out of the database only what you need.  So, ideally, you would query
 * a count of the data you want, and then feed the count into the object and get a LIMIT clause. 
 * Then pull out the specific data you want for that page with the LIMIT clause in your query.
 */

class PageLogic
{

/* PUBLIC MEMBERS */

    public $current;            // Current set page number
    public $last;               // Previous page number 
    public $next;               // Next page number
    public $page_numbers;       // Array of all page numbers
    public $sql_start;          // START value to use in a SQL LIMIT clause
    public $sql_limit;          // LIMIT value "    "   "   "   "   "   "
    public $add_query;          // Holds additional key/var pairings as a HTTP query


/* PRIVATE MEMBERS */

    private $mCount;            // The count of items to be paginated
    private $mCalculated;       // The object's state of being calculated


/* PUBLIC METHODS */

    public function __construct()
    // Initialize the object
    {
        $this->current = NULL;
        $this->last = NULL;
        $this->next = NULL;
        $this->sql_start = '0';
        $this->sql_limit = '0';
        $this->page_numbers = array();
        $this->mCount = 0;
        $this->mCalculated = FALSE;
        $this->add_query = "";
    }

    public function SetItemCount($count)
    // Set the count of the items to be paginated
    {
        $this->mCount = $count;
    }

    public function SetCurrentPageNumber($page_no)
    // Set the current page number to generate other values from
    {
        $this->current = ($page_no < 1) ? 1 : $page_no;
    }

    public function AddQueryVar($key, $val)
    // Add a key/val pair as a query to be added onto the end of HTTP links to other pages.
    // This is useful for passing vital bits of information between pages when using the
    // page_nav.tpl (optionally, you can use your own custom template)
    {
        $this->add_query .= "&" . urlencode($key) . '=' . urlencode($val);
    }

    public function Calculate()
    // Calculate the various values: last page number, number of pages, previous and next pages,
    // amount of pages.  Also generates corresponding values to use for pulling from the SQL DB.
    {
        if (!$this->mCalculated)
        {
            $this->last = floor($this->mCount / MceConfig::RECORDS_PER_PAGE);
            if ($this->mCount % MceConfig::RECORDS_PER_PAGE > 0)
                $this->last += 1;
            if ($this->last == 0)
                $this->last = 1;
            if ($this->current > $this->last)
                $this->current = $this->last;
            $this->previous = $this->current - 1;
            $this->next = $this->current + 1;
            $this->page_numbers = range(1,$this->last);
            $this->sql_start = ($this->previous) * MceConfig::RECORDS_PER_PAGE;
            $this->sql_limit = MceConfig::RECORDS_PER_PAGE;
        }
    }

    public function GetSqlLimit()
    // Generates the SQL LIMIT clause itself.  Saves a little trouble.
    {
        $this->Calculate();
        return ' LIMIT ' . $this->sql_start . ', ' . $this->sql_limit;
    }

}


?>