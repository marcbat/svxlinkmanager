###############################################################################
#
# Locale specific functions for playing back time, numbers and spelling words.
# Often, the functions in this file are the only ones that have to be
# reimplemented for a new language pack.
#
###############################################################################

#
# FRANCAIS	FRANCAIS	FRANCAIS	FRANCAIS	FRANCAIS	FRANCAIS	FRANCAIS	FRANCAIS
#


#
# Say the specified two digit number (00 - 99)
#
proc playTwoDigitNumber {number} {
	if {[string length $number] != 2} {
		puts "*** WARNING: Function playTwoDigitNumber received a non two digit number: $number";
		return;
	}

	set first [string index $number 0];
  
	# 01 => 09
	if {($first == "0") || ($first == "O")} {
		#playMsg "Default" $first;
		playMsg "Default" [string index $number 1];
	
	# 10 => 20, 30, 40, 50, 60, 70, 80, 90
    } elseif {$first == "1" || [string index $number 1] == "0" } {
        playMsg "Default" $number;

	# 70 => 79
    } elseif {$first == "7"} {
        playMsg "Default" "6X";
        if {[string index $number 1] == "1"} {
			playMsg "Default" "and";
        }
        
		playMsg "Default" [expr $number - 60];
	
    # 90 => 99
	} elseif {$first == "9"}  {
        playMsg "Default" "8X";
        playMsg "Default" [expr $number - 80];
    
	} else {
		playMsg "Default" "[string index $number 0]X";
        if {[string index $number 1] == "1"} {
			playMsg "Default" "and";
        }
        
		playMsg "Default" "[string index $number 1]";
				
    }
}




#
# Say the specified three digit number (000 - 999)
#
proc playThreeDigitNumber {number} {
  if {[string length $number] != 3} {
    puts "*** WARNING: Function playThreeDigitNumber received a non three digit number: $number";
    return;
  }
  
  set first [string index $number 0];
  
  if {($first == "0") || ($first == "O")} {
    playTwoDigitNumber [string range $number 1 2];
 
  } else {
  
    append first "00";
    playMsg "Default" $first;
    
	if {[string index $number 1] != "0"} {
      playTwoDigitNumber [string range $number 1 2];
    } elseif {[string index $number 2] != "0"} {
      playMsg "Default" [string index $number 2];
    }
  }
}



#
# Say the specified four digit number (1000 - 9999)
#
proc playFourDigitNumber {number} {
	if {[string length $number] != "4"} {
		puts "*** WARNING: Function playFourDigitNumber received a non four digit number: $number";
		return;
	}
 
	set first [string index $number 0];
    
	if {($first == "1")} {
		playMsg "Default" "1000";
	} elseif {($first == "2")} {
		playMsg "Default" "2000";
	} elseif {($first == "3")} {
		playMsg "Default" "3000";
	} elseif {($first == "4")} {
		playMsg "Default" "4000";
	} elseif {($first == "5")} {
		playMsg "Default" "5000";
	} elseif {($first == "6")} {
		playMsg "Default" "6000";
	} elseif {($first == "7")} {
		playMsg "Default" "7000";
	} elseif {($first == "8")} {
		playMsg "Default" "8000";
	} elseif {($first == "9")} {
		playMsg "Default" "9000";
	}
  
	if {[string index $number 1] != "0"} {
		playThreeDigitNumber [string range $number 1 3];
	} elseif {[string index $number 2] != "0"} {
		playTwoDigitNumber [string range $number 2 3];
	} elseif {[string index $number 3] != "0"} {
		playMsg "Default" [string index $number 3]
	}
}






#
# Say a number as intelligent as posible. Examples:
#
#	1	- one
#	24	- twentyfour
#	245	- twohundred and fourtyfive
#	1234	- one thousand two hundred and thirtyfour
#	12345	- twelwe thousand three hundred and fourtyfive
#	136.5	- onehundred and thirtysix point five
#
proc playNumber {number} {

	if {[regexp {(\d+)\.(\d+)?} $number -> integer fraction]} {
		playNumber $integer;
		# No say 0 if decimal = 0
		if {$fraction != 0} {
			playMsg "Default" "decimal";
			spellNumber $fraction;
		}
    
		return;
	}

	set len [string length $number];
		
	
	if {$len == 1} {
      playMsg "Default" $number;
	} elseif {$len == 2} {
      playTwoDigitNumber $number;
	} elseif {$len == 3} {
      playThreeDigitNumber $number
	} else {
	  playFourDigitNumber $number
	}

}


#
# Say the time specified by function arguments "hour" and "minute".
#

proc playTime {hour minute} {
	# Strip white space and leading zeros. Check ranges.
	if {[scan $hour "%d" hour] != 1 || $hour < 0 || $hour > 23} {
		error "playTime: Non digit hour or value out of range: $hour"
	}
	if {[scan $minute "%d" minute] != 1 || $minute < 0 || $minute > 59} {
		error "playTime: Non digit minute or value out of range: $hour"
	}
  
	if {[string length $hour] == 1} {
		playMsg "Default" [expr $hour];
	} else {
     playTwoDigitNumber $hour;
    }
	#  playSilence 250
	playMsg "Core" "time";
 
    if {$minute == 0} {
		return;
    }
    if {[string length $minute] == 1} {
		playMsg "Default" 0;
        playMsg "Default" [expr $minute];
    } else {
        playTwoDigitNumber  $minute;
    }
}


#
# This file has not been truncated
#
